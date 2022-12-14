using System.Text.Json;
using DFrame.Controller;

namespace Rabbit;

/// <summary>
/// 結果をjsonで出力するクラス
/// Rabbit/Rabbit/bin/Debug/net6.0/中の指定したディレクトリに出力されます。
/// コマンドラインからdotnet runした場合はRabbit/下の指定したディレクトリに出力されます。
/// </summary>
public class FlatFileLogExecutionResultHistoryProvider : IExecutionResultHistoryProvider
{
    /// <summary>
    /// 出力ディレクトリ
    /// </summary>
    readonly string rootDir;
    
    /// <summary>
    /// IExecutionResultHistoryProviderインターフェース
    /// </summary>
    readonly IExecutionResultHistoryProvider memoryProvider;
    
    /// <summary>
    /// イベントハンドラ
    /// </summary>
    public event Action? NotifyCountChanged;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="rootDir">出力ディレクトリ</param>
    public FlatFileLogExecutionResultHistoryProvider(string rootDir)
    {
        this.rootDir = rootDir;
        this.memoryProvider = new InMemoryExecutionResultHistoryProvider();
    }

    public int GetCount()
    {
        return memoryProvider.GetCount();
    }

    public IReadOnlyList<ExecutionSummary> GetList()
    {
        return memoryProvider.GetList();
    }

    public (ExecutionSummary Summary, SummarizedExecutionResult[] Results)? GetResult(DFrame.Controller.ExecutionId executionId)
    {
        return memoryProvider.GetResult(executionId);
    }

    /// <summary>
    /// json出力
    /// </summary>
    /// <param name="summary"></param>
    /// <param name="results"></param>
    public void AddNewResult(ExecutionSummary summary, SummarizedExecutionResult[] results)
    {
        var fileName = $"{summary.StartTime.ToString("yyyy-MM-dd hh.mm.ss")} {summary.Workload} {summary.ExecutionId}";
        var json = JsonSerializer.Serialize(new { summary, results }, new JsonSerializerOptions { WriteIndented = true });

        var d = Directory.CreateDirectory(rootDir);
        Console.WriteLine(d.FullName);
        File.WriteAllText(Path.Combine(rootDir, fileName), json);

        memoryProvider.AddNewResult(summary, results);
        NotifyCountChanged?.Invoke();
    }
}