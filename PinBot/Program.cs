using DSharpPlus;

string? pinRoleIdString = Environment.GetEnvironmentVariable("ROLE_ID");
string environmentVariable = (Environment.GetEnvironmentVariable("IGNORE_CHANNELS") ?? "");
string[] strings = environmentVariable.Split(";", StringSplitOptions.RemoveEmptyEntries);
ulong[] ignoredChannelIds = strings.Select(ulong.Parse).ToArray();
ulong? pinRoleId = pinRoleIdString == null ? null : ulong.Parse(pinRoleIdString);

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN")
});

await discord.ConnectAsync();

discord.MessageReactionAdded += async (_, args) => {
	if (ignoredChannelIds.Contains(args.Channel.Id)) {
		return;
	}
	
	if (args.Emoji.Name == "📌" && (args.Guild == null || !pinRoleId.HasValue || (await args.Guild.GetMemberAsync(args.User.Id)).Roles.Any(role => role.Id == pinRoleId))) {
		await args.Message.PinAsync();
	} else if (args.Emoji.Name == "⛔" && args.Message.Author.Id == args.User.Id) {
		await args.Message.UnpinAsync();
	}
};

await Task.Delay(-1);
