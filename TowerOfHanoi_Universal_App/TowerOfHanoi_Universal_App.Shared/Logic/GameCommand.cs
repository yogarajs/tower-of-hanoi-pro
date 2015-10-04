using System;
using System.Windows.Input;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Command class to bind commands.
    /// </summary>
    public class GameCommand : ICommand
    {
        #region Members

        readonly Action _execute;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public GameCommand(Action execute)
        {
            _execute = execute;
        }

        #endregion

        #region ICommand Members

        /// <inheritdoc select="*"/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc select="*"/>
        public bool CanExecute(object parameter)
        {
            // Always return true. Since we have separate logic whether to execute or not.
            return true;
        }

        /// <inheritdoc select="*"/>
        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion
    }
}
