using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

string? pinRoleIdString = Environment.GetEnvironmentVariable("ROLE_ID");
string environmentVariable = (Environment.GetEnvironmentVariable("IGNORE_CHANNELS") ?? "");
string[] strings = environmentVariable.Split(";", StringSplitOptions.RemoveEmptyEntries);
ulong[] ignoredChannelIds = strings.Select(ulong.Parse).ToArray();
ulong? pinRoleId = pinRoleIdString == null ? null : ulong.Parse(pinRoleIdString);

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN")
});

await discord.ConnectAsync();

async Task OnMessageReactionAdded(DiscordClient _, MessageReactionAddEventArgs args) {
	if (ignoredChannelIds.Contains(args.Channel.Id)) {
		return;
	}

	DiscordMessage message = args.Message;
	if (message.Author == null) {
		Console.WriteLine("YEAH");
		message = await args.Channel.GetMessageAsync(args.Message.Id);
	}
	
	if (args.Emoji.Name == "📌") {
		if (args.Guild == null || !pinRoleId.HasValue || (await args.Guild.GetMemberAsync(args.User.Id)).Roles.Any(role => role.Id == pinRoleId)) {
			await message.PinAsync();
		}
	} else if (args.Emoji.Name == "⛔") {
		if (message.Author.Id == args.User.Id) {
			await message.UnpinAsync();
		}
	}
}

discord.MessageReactionAdded += OnMessageReactionAdded;

await Task.Delay(-1);
