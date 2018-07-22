# LazyCube

This is a fork of Emzi0767's Companion Cube bot. It uses a half-implemented in-memory database
instead of PostgreSQL, and comes with Lavalink, and the JSON data pre-generated.

## Setup

Build and run it. It will complain about not having a config.json file, and will create it. Change
the currency symbol, and add your bot token. Run it again. It will likely complain about Lavalink
not being present. Copy the [application.yml.example](https://github.com/Frederikam/Lavalink/blob/master/LavalinkServer/application.yml.example)
file to the root of the solution, and fill it with your things. You probably want a secure password,
if you intend on running Lavalink through port 80, or another port that is allowed in your firewall.
Run `java -jar Lavalink.jar`. Run the bot again. The bot should work. If the bot does not work, take
the nearest door to your left, then go right at the station, take the first train, wait for the
second one heading east. Take it, find the pretty girl with gold teeth, she will give you a hotel
key with your name on it. Wait until the train hits the border in the morning, leave through the
back door before they have time to ask you for your ticket. Welcome to Delphi.

## FAQ

#### Is it possible to have Visual Studio start Lavalink when I start the bot?
Yes.

#### Some of the commands don't work!
That's not a question.

#### OK, why do some of the commands not work?
The emote names used refer to guild emotes that you likely don't have. Either create them, or
replace them with Discord defaults, or your own.

#### It doesn't save when I restart the bot!
I find that turning it off and on again is overrated, and I want to discourage it. Also, I didn't
want to make a database.

#### This sucks!
Just relax, everything is going to be alright.

## Acknowledgements

Companion Cube was created by [Emzi0767](https://github.com/Emzi0767/Discord-Companion-Cube-Bot).
Give him your money. All of it.