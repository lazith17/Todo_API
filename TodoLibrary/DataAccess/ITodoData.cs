using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoLibrary.Models;

namespace TodoLibrary.DataAccess
{
    public interface ITodoData
    {
        Task CompleteTodo(int assignedTo, int todoId);
        Task UndoComplete(int assignedTo, int todoId);
        Task<TodoModel?> Create(int assignedTo, string task);
        Task Delete(int assignedTo, int todoId);
        Task<List<TodoModel>> GetAllAssigned(int assignedTo);
        Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId);
        Task UpdateTask(int assignedTo, int todoId, string task);
    }
}