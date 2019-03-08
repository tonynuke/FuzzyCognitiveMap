﻿using System;
using System.Windows.Input;

namespace FuzzyCognitiveModel
{
    /// <summary>
    /// Команда.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Действие команды.
        /// </summary>
        public Action<object> ExecuteAction { get; private set; }

        /// <summary>
        /// Обработчик возможности выполнения команды.
        /// </summary>
        public Func<object, bool> CanExecuteFunction { get; private set; }

        /// <summary>
        /// Событие изменения признака воможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            ExecuteAction?.Invoke(parameter);
        }

        /// <summary>
        /// Получить признак возможности выполнения команды.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>Признак возможности выполнения команды.</returns>
        public bool CanExecute(object parameter)
        {
            return this.CanExecuteFunction == null || this.CanExecuteFunction(parameter);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="executeAction">Действие команды.</param>
        public DelegateCommand(Action<object> executeAction) : this(executeAction, null)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="executeAction">Действие команды.</param>
        /// <param name="canExecuteFunction">Обработчик возможности выполнения команды.</param>
        public DelegateCommand(Action<object> executeAction,
            Func<object, bool> canExecuteFunction)
        {
            this.ExecuteAction = executeAction;
            this.CanExecuteFunction = canExecuteFunction;
        }
    }
}