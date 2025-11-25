using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SG001
{
    [Generator]
    public class DomainEventGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 步驟 1: 找出所有標記 [DomainEntity] 的類別
            var entityClasses = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "PartialWithSG001.DomainEntityAttribute",
                    predicate: static (s, _) => IsCandidateClass(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);

            // 步驟 2: 為每個找到的類別生成程式碼
            context.RegisterSourceOutput(entityClasses, static (spc, source) => Execute(source!, spc));
        }

        /// <summary>
        /// 檢查節點是否為候選類別 (有 attribute 的 class)
        /// </summary>
        private static bool IsCandidateClass(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax;
        }

        /// <summary>
        /// 使用語意模型檢查是否標記 [DomainEntity]
        /// </summary>
        private static INamedTypeSymbol? GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context)
        {
            return context.TargetSymbol as INamedTypeSymbol;
        }

        /// <summary>
        /// 執行程式碼生成
        /// </summary>
        private static void Execute(INamedTypeSymbol typeSymbol, SourceProductionContext context)
        {
            // 取得類別資訊
            var className = typeSymbol.Name;
            var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

            // 檢查是否為 partial class
            if (!typeSymbol.DeclaringSyntaxReferences.Any(sr =>
                sr.GetSyntax() is ClassDeclarationSyntax cds &&
                cds.Modifiers.Any(m => m.Text == "partial")))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "DEGEN001",
                        "Class must be partial",
                        "Class '{0}' must be declared as partial to use DomainEntity attribute",
                        "DomainEventGenerator",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                    typeSymbol.Locations.FirstOrDefault(),
                    className));
                return;
            }

            // 收集所有 partial events
            var events = new List<EventInfo>();

            foreach (var member in typeSymbol.GetMembers())
            {
                if (member is IEventSymbol eventSymbol)
                {
                    // 檢查是否為 partial event
                    var syntaxRef = eventSymbol.DeclaringSyntaxReferences.FirstOrDefault();
                    if (eventSymbol.IsPartialDefinition)
                    {
                        var eventType = eventSymbol.Type.ToDisplayString();
                        var eventName = eventSymbol.Name;
                        // 提取 EventHandler<T> 的 T 型別
                        string eventArgsType = null;
                        if (eventSymbol.Type is INamedTypeSymbol namedType &&
                            namedType.IsGenericType &&
                            namedType.TypeArguments.Length > 0)
                        {
                            eventArgsType = namedType.TypeArguments[0].Name;
                        }
                        events.Add(new EventInfo(eventName, eventType, eventArgsType));
                    }
                
                    //if (syntaxRef?.GetSyntax() is EventDeclarationSyntax eventSyntax &&
                    //    eventSyntax.Modifiers.Any(m => m.Text == "partial"))
                    //{
                    //    var eventType = eventSymbol.Type.ToDisplayString();
                    //    var eventName = eventSymbol.Name;

                    //    // 提取 EventHandler<T> 的 T 型別
                    //    string? eventArgsType = null;
                    //    if (eventSymbol.Type is INamedTypeSymbol namedType &&
                    //        namedType.IsGenericType &&
                    //        namedType.TypeArguments.Length > 0)
                    //    {
                    //        eventArgsType = namedType.TypeArguments[0].Name;
                    //    }

                    //    events.Add(new EventInfo(eventName, eventType, eventArgsType));
                    //}
                }
            }

            // 生成程式碼
            var source = GenerateSource(namespaceName, className, events);

            // 加入到編譯中
            context.AddSource($"{className}.g.cs", source);
        }

        /// <summary>
        /// 生成完整的 partial class 程式碼
        /// </summary>
        private static string GenerateSource(string namespaceName, string className, List<EventInfo> events)
        {
            // 生成 partial events 的實作
            var eventsCode = GeneratePartialEvents(events);

            // 生成事件訂閱程式碼
            var eventSubscriptions = GenerateEventSubscriptions(events);

            // 生成事件處理器
            var eventHandlers = GenerateEventHandlers(events);

            return $$"""
            // <auto-generated/>
            // 此檔案由 DomainEventGenerator 自動生成
            // 請勿手動修改此檔案
            
            using System;
            using System.Collections.Generic;
            using System.Linq;
            
            namespace {{namespaceName}};
            
            public partial class {{className}}
            {
                // === 事件基礎設施欄位 ===
                private readonly List<(string EventName, DateTime Timestamp, object EventArgs)> _eventHistory = new();
                private readonly List<string> _eventLog = new();
            
            {{eventsCode}}
            
                // === Partial Constructor 實作 ===
                public partial {{className}}()
                {
                    InitializeDomainEvents();
                }
            
                /// <summary>
                /// 初始化領域事件系統
                /// </summary>
                private void InitializeDomainEvents()
                {
                    Console.WriteLine($"[領域事件系統] 初始化實體: {GetType().Name}");
            
            {{eventSubscriptions}}
                }
            
            {{eventHandlers}}
            
                // === 公開查詢方法 ===
                /// <summary>
                /// 取得完整的事件歷史
                /// </summary>
                public IReadOnlyList<(string EventName, DateTime Timestamp, object EventArgs)> GetEventHistory()
                {
                    return _eventHistory.AsReadOnly();
                }
            
                /// <summary>
                /// 取得事件日誌
                /// </summary>
                public IReadOnlyList<string> GetEventLog()
                {
                    return _eventLog.AsReadOnly();
                }
            
                /// <summary>
                /// 取得特定類型的事件
                /// </summary>
                public IEnumerable<(string EventName, DateTime Timestamp, object EventArgs)> GetEventsByName(string eventName)
                {
                    return _eventHistory.Where(e => e.EventName == eventName);
                }
            
                /// <summary>
                /// 列印事件歷史到控制台
                /// </summary>
                public void PrintEventHistory()
                {
                    Console.WriteLine($"\n=== {GetType().Name} 事件歷史 ===");
                    foreach (var log in _eventLog)
                    {
                        Console.WriteLine(log);
                    }
                    Console.WriteLine($"總計: {_eventHistory.Count} 個事件\n");
                }
            
                /// <summary>
                /// 清除所有事件歷史
                /// </summary>
                public void ClearEventHistory()
                {
                    _eventHistory.Clear();
                    _eventLog.Clear();
                    Console.WriteLine($"[領域事件系統] 已清除 {GetType().Name} 的事件歷史");
                }
            }
            """;
        }

        /// <summary>
        /// 生成 partial events 的實作
        /// </summary>
        private static string GeneratePartialEvents(List<EventInfo> events)
        {
            if (events.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.AppendLine("    // === Partial Events 實作 ===");

            foreach (var evt in events)
            {
                sb.Append($$"""
                    private {{evt.EventType}} _{{evt.EventName}};
                
                    public partial event {{evt.EventType}} {{evt.EventName}}
                    {add { _{{evt.EventName}} += value; }
                        remove { _{{evt.EventName}} -= value; }
                    }
                
            """);

                // 如果有 EventArgsType，生成強型別的 OnXxx 方法
                if (!string.IsNullOrEmpty(evt.EventArgsType))
                {
                    sb.Append($$"""
                
                        /// <summary>
                        /// 觸發 {{evt.EventName}} 事件的輔助方法
                        /// </summary>
                        protected void On{{evt.EventName}}({{evt.EventArgsType}} e)
                        {
                            _{{evt.EventName}}?.Invoke(this, e);
                        }   
                """);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成事件訂閱程式碼
        /// </summary>
        private static string GenerateEventSubscriptions(List<EventInfo> events)
        {
            if (events.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.AppendLine("        // 自動訂閱事件處理器");

            foreach (var evt in events)
            {
                sb.AppendLine($"        _{evt.EventName} += On{evt.EventName}Raised;");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成事件處理器
        /// </summary>
        private static string GenerateEventHandlers(List<EventInfo> events)
        {
            if (events.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.AppendLine("    // === 自動生成的事件處理器 ===");

            foreach (var evt in events)
            {
                sb.Append($$"""
                    /// <summary>
                    /// {{evt.EventName}} 事件的自動處理器
                    /// </summary>
                    private void On{{evt.EventName}}Raised(object? sender, EventArgs e)
                    {
                        var timestamp = DateTime.UtcNow;
                        _eventHistory.Add(("{{evt.EventName}}", timestamp, e));
                        var logMessage = $"[{timestamp:yyyy-MM-dd HH:mm:ss}] {{evt.EventName}} 已發布";
                        _eventLog.Add(logMessage);
                        Console.WriteLine($"[領域事件] {logMessage}");
                    }
                
                
            """);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 事件資訊記錄
        /// </summary>
        private record struct EventInfo(string EventName, string EventType, string? EventArgsType);
    }
}