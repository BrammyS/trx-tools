using System;
using System.Collections.Generic;
using System.Text;

namespace trx_tools.Commands
{
    public class CLIArgHandler
    {
        private string[] args;
        public int Length => args.Length;

        public CLIArgHandler(String[] args)
        {
            this.args = args;
        }
        public bool GetFlag(String name) => args.Any(a => a.Equals($"--{name}", StringComparison.OrdinalIgnoreCase));

        public string? GetOpt(String name) => GetOptArr(name, 1).FirstOrDefault();

        public string[] GetInitialUnnamedArgs()
        {
            var ret = new List<string>();
            foreach (var arg in args) {
                if (arg.StartsWith("--"))
                    break;
                ret.Add(arg);
            }
            
            return ret.ToArray();
        }

        public string[] GetOptArr(String name, int max = 0)
        {
            var ret = new List<string>();
            var capture = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals($"--{name}", StringComparison.OrdinalIgnoreCase))
                {
                    capture = true;
                    continue;
                }
                if (capture)
                {
                    if (args[i].StartsWith("--"))
                    {
                        capture = false;
                        continue;
                    }
                    ret.Add(args[i]);
                    if (max > 0 && ret.Count == max)
                        break;

                }
            }
            return ret.ToArray();
        }
    }
}
