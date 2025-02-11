namespace trx_tools.Core.Models;

public record Times(
    DateTimeOffset Creation,
    DateTimeOffset Queuing,
    DateTimeOffset Start,
    DateTimeOffset Finish
);