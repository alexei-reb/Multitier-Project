using System;

namespace ApplicationServer
{
    public class SocketEventArgs: EventArgs
    {
        private Command command;

        public SocketEventArgs(Command command)
        {
            this.command = command;
        }

        public Command Command
        {
            get
            {
                return command;
            }
        }
    }
}
