using Microsoft.CodeAnalysis.Scripting;
using System;

namespace NiceET
{
    public class ReplComponentAwakeSystem : AwakeSystem<ReplComponent>
    {
        public override void Awake(ReplComponent self)
        {
            self.ScriptOptions = ScriptOptions.Default
                    .WithMetadataResolver(ScriptMetadataResolver.Default.WithBaseDirectory(Environment.CurrentDirectory))
                    .AddReferences(typeof(ReplComponent).Assembly)
                    .AddImports("System");
        }
    }
}
