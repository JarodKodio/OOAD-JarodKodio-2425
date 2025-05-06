using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public enum MessageType
    {
        Locked,
        CantPickUp,
        CantUse
    }
    public static class RandomMessageGenerator
    {
        private static string[] lockedMessages = new string[]
        {
            "This is locked.",
            "This is stuck.",
            "This is jammed.",
            "This is blocked."
        };

        private static string[] noPickUpMessage = new string[]
        {
            "I can't pick that up the",
            "I can't carry that",
            "I can't take that",
            "I can't lift that"
        };

        private static string[] cantUseMessages = new string[]
        {
            "I can't use that.",
            "I can't handle that.",
        };

        public static string GetRandomMessage(MessageType type)
        {
            string message = null;
            Random rnd = new Random();
            if (type == MessageType.Locked)
            {
                message = lockedMessages[rnd.Next(lockedMessages.Length)];
            }
            else if (type == MessageType.CantPickUp)
            {
                message = noPickUpMessage[rnd.Next(noPickUpMessage.Length)];
            }
            else if (type == MessageType.CantUse)
            {
                message = cantUseMessages[rnd.Next(cantUseMessages.Length)];
            }

            return message;
        }
    }
}
