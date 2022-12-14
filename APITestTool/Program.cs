using DFrame;
using DFrame.Controller;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit;

/// <summary>
/// エントリーポイント
/// </summary>
public class Program
{
    /// <summary>
    /// LOG_DIR_PATH
    /// </summary>
    private static readonly string LOG_DIR_PATH = "--logs=";

    /// <summary>
    /// 負荷テスト出力ログ
    /// </summary>
    private static string? _logPath = "";

    public static async Task Main(string[] args)
    {
        _logPath = args.FirstOrDefault(s => s.StartsWith(LOG_DIR_PATH))?.Substring(LOG_DIR_PATH.Length);
        
        // builder
        var builder = DFrameApp.CreateBuilder(7321,7313);
        
        // builderの設定
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<HttpClient>();
            // 負荷テストの結果出力先
            if (_logPath != null)
            {
                services.AddSingleton<IExecutionResultHistoryProvider>(
                    new FlatFileLogExecutionResultHistoryProvider(_logPath));

            }
        });
        
        // コメントアウトを解除するとDFrameデフォルトのメニューが出てきます。
        // builder.ConfigureWorker(w =>
        // {
        //     w.IncludesDefaultHttpWorkload = true;
        // });
        await builder.RunAsync();
    }
}