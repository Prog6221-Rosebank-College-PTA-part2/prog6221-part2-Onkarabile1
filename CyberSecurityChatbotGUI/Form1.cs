
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Media;
using System.Windows.Forms;

namespace CyberSecurityChatbotGUI
{
    public partial class Form1 : Form
    {
       
        // Voice Recording
        SoundPlayer player = new SoundPlayer("welcome.wav");

        // Memory
        string userName = "";
        string currentTopic = "";
        string userInterest = "";

        // Random responses
        Random rand = new Random();

        // Cybersecurity responses
        Dictionary<string, string[]> responses = new Dictionary<string, string[]>()
        {
            {
                "password",
                new string[]
                {
                    "Use strong passwords with symbols and numbers.",
                    "Avoid using your birthday as a password.",
                    "Always create unique passwords for each account."
                }
            },

            {
                "phishing",
                new string[]
                {
                    "Do not click suspicious email links.",
                    "Be careful of fake emails pretending to be trusted companies.",
                    "Always verify email senders before clicking links."
                }
            },

            {
                "malware",
                new string[]
                {
                    "Install trusted antivirus software.",
                    "Keep your system updated to prevent malware attacks.",
                    "Avoid downloading files from untrusted websites."
                }
            },

            {
                "virus",
                new string[]
                {
                    "Run regular antivirus scans.",
                    "Do not open suspicious attachments.",
                    "Keep your operating system updated."
                }
            },

            {
                "hacker",
                new string[]
                {
                    "Use two-factor authentication for better security.",
                    "Never share personal information online carelessly.",
                    "Hackers often target weak passwords."
                }
            }
        };

        public Form1()
        {
            InitializeComponent();

            
            // Play welcome sound
            try
            {
                player.Play();
            }
            catch
            {
                MessageBox.Show("welcome.wav file not found.");
            }

            // Welcome message
            rtbChat.AppendText("Bot: Welcome to the Cybersecurity Awareness Chatbot!\n\n");

            // ENTER key sends message
            txtUserInput.KeyDown += TxtUserInput_KeyDown;
        }

        // ENTER KEY EVENT
        private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        // RANDOM RESPONSE METHOD
        private string GetRandomResponse(string keyword)
        {
            string[] possibleResponses = responses[keyword];
            return possibleResponses[rand.Next(possibleResponses.Length)];
        }

        // SEND BUTTON
        private void btnSend_Click(object sender, EventArgs e)
        {
            // Get user message
            string userMessage = txtUserInput.Text.Trim().ToLower();

            // Empty input handling
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            // Display user message
            rtbChat.AppendText("You: " + txtUserInput.Text + "\n");

            // Bot response
            string botResponse = "";

            // MEMORY FEATURE
            if (userMessage.StartsWith("my name is"))
            {
                userName = userMessage.Replace("my name is", "").Trim();
                botResponse = "Nice to meet you " + userName + "!";
            }

            // GREETING
            else if (userMessage.Contains("hello") || userMessage.Contains("hi"))
            {
                if (userName != "")
                {
                    botResponse = "Hello again " + userName + "! How can I help you today?";
                }
                else
                {
                    botResponse = "Hello! How can I help you with cybersecurity today?";
                }
            }

            // USER INTEREST MEMORY
            else if (userMessage.Contains("privacy"))
            {
                userInterest = "privacy";
                botResponse = "Great! I will remember that you are interested in privacy.";
            }

            // WORRIED / SCARED
            else if (userMessage.Contains("worried") ||
                     userMessage.Contains("scared") ||
                     userMessage.Contains("afraid"))
            {
                botResponse = "It is understandable to feel worried about online threats.\n\n";

                if (userMessage.Contains("scam") || userMessage.Contains("phishing"))
                {
                    currentTopic = "phishing";
                    botResponse += "Tip: " + GetRandomResponse("phishing");
                }

                else if (userMessage.Contains("password"))
                {
                    currentTopic = "password";
                    botResponse += "Tip: " + GetRandomResponse("password");
                }

                else
                {
                    botResponse += "Always avoid suspicious links and keep your accounts secure.";
                }
            }

            // FRUSTRATED
            else if (userMessage.Contains("frustrated") ||
                     userMessage.Contains("angry"))
            {
                botResponse = "Cybersecurity can feel overwhelming sometimes.\n\n";
                botResponse += "Tip: Use strong passwords and enable two-factor authentication.";
            }

            // CURIOUS
            else if (userMessage.Contains("curious"))
            {
                botResponse = "That is great! Staying curious helps you stay safe online.";
            }

            // HAPPY / THANKFUL
            else if (userMessage.Contains("happy") ||
                     userMessage.Contains("thanks") ||
                     userMessage.Contains("thank you"))
            {
                botResponse = "I am glad I could help!";
            }

            // CONVERSATION FLOW
            else if (userMessage.Contains("tell me more") ||
                     userMessage.Contains("explain more") ||
                     userMessage.Equals("more"))
            {
                if (currentTopic == "phishing")
                {
                    botResponse = "Phishing scams are fake emails or messages that trick users into giving personal information like passwords or banking details.";
                }

                else if (currentTopic == "password")
                {
                    botResponse = "Strong passwords should include uppercase letters, lowercase letters, symbols, and numbers.";
                }

                else if (currentTopic == "malware")
                {
                    botResponse = "Malware is harmful software that can damage your computer or steal personal information.";
                }

                else if (currentTopic == "virus")
                {
                    botResponse = "A virus spreads between files and programs and may damage your device.";
                }

                else if (currentTopic == "hacker")
                {
                    botResponse = "Hackers try to gain unauthorized access to systems to steal or damage information.";
                }

                else
                {
                    botResponse = "Please ask about a cybersecurity topic first.";
                }
            }

            // KEYWORD RECOGNITION
            else
            {
                bool keywordFound = false;

                foreach (var item in responses)
                {
                    if (userMessage.Contains(item.Key))
                    {
                        currentTopic = item.Key;
                        botResponse = GetRandomResponse(item.Key);

                        keywordFound = true;
                        break;
                    }
                }

                // PERSONALIZED MEMORY
                if (userInterest == "privacy" && keywordFound)
                {
                    botResponse += " Since you are interested in privacy, remember to secure your personal information online.";
                }

                // DEFAULT RESPONSE
                if (!keywordFound)
                {
                    botResponse = "I am not sure I understand. Please ask a cybersecurity-related question.";
                }
            }

            // DISPLAY BOT RESPONSE
            rtbChat.AppendText("Bot: " + botResponse + "\n\n");

            // CLEAR TEXTBOX
            txtUserInput.Clear();
        }
        
        // RichTextBox TextChanged Event
        private void rtbChat_TextChanged(object sender, EventArgs e)
        {

        }

        // TextBox TextChanged Event
        private void txtUserInput_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
