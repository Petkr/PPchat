#include <vector>

static_assert(sizeof(int) == 4);
static_assert(sizeof(short) == 2);



using Char = short;



#pragma pack(push, 8)
template <typename T>
struct Span
{
	T* begin;
	T* end;

	constexpr operator bool() noexcept
	{
		return begin != end;
	}
};

struct RangeSpan { Span<Span<Char>> x; };
struct CharSpan { Span<Char> x; };
#pragma pack(pop)



template <typename Range>
constexpr std::add_rvalue_reference_t<decltype(*std::declval<Range>().begin)> declval_range() noexcept;



template <typename Range, typename Predicate>
constexpr Range skip_while(Range input, Predicate predicate) noexcept(noexcept(predicate(declval_range<Range>())))
{
	return { std::find_if(input.begin, input.end, negate(predicate)), input.end };
}

template <typename Range, typename Predicate>
constexpr Range remove(Range input, Predicate predicate) noexcept(noexcept(predicate(declval_range<Range>())))
{
	return { input.begin, std::remove_if(input.begin, input.end, predicate) };
}



template <typename F>
constexpr auto negate(F&& f) noexcept
{
	return[&f]<typename... T>(T &&... args)
	{
		return !std::forward<F>(f)(std::forward<T>(args)...);
	};
}



template <Char c>
constexpr inline auto is_char = [](auto x) noexcept { return x == c; };

static bool in_quotes;

constexpr inline auto is_nonspace_or_in_quotes =
[](auto c) noexcept
{
	if (c == '"')
	{
		in_quotes = true;
		return true;
	}
	else
		return c != ' ' || in_quotes;
};



std::pair<Span<Char>, Span<Char>> split_one_token(Span<Char> input)
{
	auto begin = input.begin;

	in_quotes = false;

	input = skip_while(input, is_nonspace_or_in_quotes);

	Span<Char> token{ begin, input.begin };

	return { remove(token, is_char<'"'>), skip_while(input, is_char<' '>) };
}

Span<Span<Char>> get_tokens(Span<Char> input)
{
	static std::vector<Span<Char>> ranges;

	ranges.clear();

	input = skip_while(input, is_char<' '>);

	while (input)
	{
		auto [x, y] = split_one_token(input);
		input = std::move(y);
		ranges.push_back(x);
	}

	return { ranges.data(), ranges.data() + ranges.size() };
}

extern "C" __declspec(dllexport) RangeSpan GetTokensRangeImplementation(CharSpan input)
{
	return { get_tokens(input.x) };
}
