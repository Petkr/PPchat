#include <vector>

static_assert(sizeof(int) == 4);



#pragma pack(push, 8)
template <typename T>
struct Span
{
	T* begin;
	T* end;

	constexpr operator bool()
	{
		return begin != end;
	}
};

struct RangeSpan { Span<Span<char>> x; };
struct CharSpan { Span<char> x; };
#pragma pack(pop)



template <typename Range, typename Predicate>
constexpr Range skip_while(Range input, Predicate predicate)
{
	return { std::find_if(input.begin, input.end, negate(predicate)), input.end };
}

template <typename Range, typename Predicate>
constexpr Range remove(Range input, Predicate predicate)
{
	return { input.begin, std::remove_if(input.begin, input.end, predicate) };
}



template <typename F>
constexpr auto negate(F&& f)
{
	return[&f]<typename... T>(T &&... args)
	{
		return !std::forward<F>(f)(std::forward<T>(args)...);
	};
}



template <char c>
constexpr inline auto is_char = [](auto x) { return x == c; };

static bool in_quotes;

constexpr inline auto is_nonspace_or_in_quotes =
[](auto c)
{
	if (c == '"')
	{
		in_quotes = true;
		return true;
	}
	else
		return c != ' ' || in_quotes;
};



std::pair<Span<char>, Span<char>> split_one_token(Span<char> input)
{
	auto begin = input.begin;

	in_quotes = false;

	input = skip_while(input, is_nonspace_or_in_quotes);

	Span<char> token{ begin, input.begin };

	return { remove(token, is_char<'"'>), skip_while(input, is_char<' '>) };
}

Span<Span<char>> get_tokens(Span<char> input)
{
	static std::vector<Span<char>> ranges;

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
