# PPchat
Demo chat project using the [PPnetwork](https://github.com/Petkr/PPnetwork) library.

## Basic Information

The project consists of two applications:

* PPchatClient
* PPchatServer

Both are console application that accept text-form commands via arguments.

This project is very minimalistic and serves just as a demonstration of the capabilites of PPnetwork.

## Commands Input

All control from the user in both applications is done with commands.

Tokens in input are separated with spaces not enclosed in double quotes.

If a command has '*' after it's one argument, this means that everything after the command name
in the input is treated as the argument.

#### Example

Suppose there are commands with signatures:
* x argument1 argument2
* y argument*

Inputs will be parsed in the following way:
* x abc defg - call to x with argument1 = "abc" and argument2 = "defg"
* x abc - call to x with bad argument count
* y abc defg - call to y with argument = "abc defg"

## The Client

This is the client side of the project.

The Client can connect to a server with an IP address and a port.

Then it can send messages to other connected Clients.

### Commands

If user enters an input not recognised as any of these commands,
it treats the input as if the command [say](#say-message) with the input as its argument was entered.

#### connect

Connects to the loopback IP address on the [default port](#Default-Port).

#### connect `port`

Connects to the loopback IP address on port `port`.

`port` need to be a number between 0 and 65,536.

#### connect `address`

Connects to IP address `address` on the [default port](#Default-Port).

`address` needs to be in standard IPv4 format (i.e. 0.0.0.0 - 255.255.255.255).

This command might also accept IPv6, but this is untested.

#### connect `address` `port`

Connects to the IP address `address` on port `port`.

#### connect `server_name`

Connects to a [saved server](#Saved-Servers) named `server_name`.

#### disconnect

Disconnects from the server.

#### port

Prints the [default port](#Default-Port).

#### port `port`

Changes the [default port](#Default-Port) to `port`.

#### save `address` `port` `server_name`

Saves a server to [saved servers](#Saved-Servers) with name `server_name`, address `address` and port `port`.

#### saved_servers

Prints all [saved servers](#Saved-Servers).

#### say `message`*

Sends `message` to the server, which sends it to other connected clients.

### Default Port

The user can set a default port, which is used as default in some commands.

This setting is saved on disk when the client closes.

### Saved Servers

The user can save some server addresses and ports under a custom name with
[save](#save-address-port-server_name) command.

This setting is saved on disk when the client closes.

## The Server

This is the server side of the project.

It listens to Clients.

### Commands

#### start

Starts the server and listens on port 2048.

#### start `port`

Starts the server and listens on port `port`.

#### stop

Stops the server.

## Implementation Details

[here](DETAILS)

## License

[MIT License](LICENSE)
