﻿namespace TelegramBot.WorkerService;

public interface ITelegramWorker
{
    public Task StartAsync();
}