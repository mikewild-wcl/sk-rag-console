using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Rag.Application.DocumentLoaders.Interfaces;

public interface IChatService
{
    public Task<string> ChatAsync(string prompt);
}
