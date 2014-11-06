using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Hang Man - A pair-programming project between Reid and Zachary
 * Stickman ASCII Art function and some design changes added by Reid.
 */


namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            Hangman();
        }

        /// <summary>
        /// Play Hangman. Input handled during loop.
        /// </summary>
        static void Hangman()
        {
            // initializing variables
            List<string> wordList = new List<string>();
            wordList = "apple blackbird pear computer driver library water backpack".ToUpper().Split().ToList();
            // when playing is set to false, the function will end
            bool playing = true;
            Random rng = new Random();
            // this is the user's reply to the playAgain prompt
            char playAgain = ' ';

            Console.Write("What is your name? ");
            string name = Console.ReadLine();

            while (playing)
            {
                // these variables are reset upon a new game
                int guessesLeft = 8;

                string randomWord = wordList[rng.Next(0, wordList.Count())];
                string guessed = string.Empty; //Contains letters that the user has guessed so far.
                string input = string.Empty; //User input is contained here.

                string message = "Welcome, " + name + "!"; //Messages are shown right above the input field every loop.

                // loop while there are guesses left and the word hasn't been guessed
                while ((guessesLeft > 0) && !WordGuessed(randomWord, guessed))
                {
                    // clears console window every loop
                    Console.Clear();
                    // The first argument of printStickMan is a string to put in the base of the structure. This
                    // implementation passes in the masked word.
                    Console.WriteLine("\n\n\n" + printStickman(MaskedWord(randomWord, guessed), guessesLeft));

                    Console.Write("\n\nGuessed so far: ");
                    // places a space between each character that is guessed
                    foreach (char i in guessed)
                        Console.Write("{0} ", i);

                    Console.WriteLine("\n\nGuesses left: {0}", guessesLeft);

                    Console.Write("\n{0}\nGuess a word or a letter: ", message);

                    // makes the input upper case
                    input = Console.ReadLine().ToUpper();

                    if (isValid(input))
                    {
                        // Single letter entered
                        if (input.Length == 1)
                        {
                            // Check to see if they have already guessed input
                            if (guessed.Contains(input))
                                message = "You have already guessed \"" + input + "\"!";
                            else
                            {
                                // checks if the letter inputted is in the word
                                if (randomWord.Contains(input))
                                {
                                    guessed += input;
                                    message = "Correct! " + input + " is in the word!";
                                }
                                // the letter is NOT in the word
                                else
                                {
                                    guessed += input;
                                    guessesLeft--;
                                    message = "Oh no! " + input + " is not in the word!";
                                }
                            }
                        }
                        else // User entered more than one character (Word guess)
                        {
                            if (input.ToUpper() == randomWord)
                            {
                                // guessed cannot have duplicate characters
                                foreach (char i in input)
                                    if (!guessed.Contains(i))
                                    {
                                        guessed += i;
                                    }
                            }
                            // input is NOT the word and decrement guess counter
                            else
                            {
                                if (!(input == "")) //Do nothing if input is "". Message will remain the same.
                                {
                                    message = "Nope! " + input + " is not the word!";
                                    guessesLeft--;
                                }
                            }
                        }

                    }
                    // tells the user the input is not valid
                    else
                        message = input + " is not a valid input. Try again.";

                    // lose condition
                    if (guessesLeft == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n\n" + printStickman(MaskedWord(randomWord, guessed), guessesLeft));
                        Console.WriteLine("\n\nYou lose!");
                        Console.WriteLine("The word was: {0}", randomWord);
                    }
                    // win condition
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n\n" + printStickman(MaskedWord(randomWord, guessed), 9)); //9 returns the
                        Console.WriteLine("\n\nYou win!");                                               //victory screen.
                    }
                }// end of game loop

                // ask if user wants to play again
                Console.Write("Do you want to play again? (Y, or any other key to exit): ");
                // playAgain will contain their next keystroke
                playAgain = Console.ReadKey().KeyChar;
                if (!(playAgain == 'Y' || playAgain == 'y'))
                    playing = false;

            }// end of playing loop
        }

        /// <summary>
        /// Checks to see if user guessed the word yet
        /// </summary>
        /// <param name="targetWord">Word to guess</param>
        /// <param name="guessedSoFar">The string that contains the characters already guessed. Do not have repeated characters in the string.</param>
        /// <returns>True if word has been guessed</returns>
        static bool WordGuessed(string targetWord, string guessedSoFar)
        {
            int counter = 0;
            string correctLetters = string.Empty;

            for (int i = 0; i < targetWord.Length; i++)
            {
                // forms a new string that is targetWord without repeating letters
                if (!correctLetters.Contains(targetWord[i]))
                    correctLetters += targetWord[i];
            }

            for (int i = 0; i < guessedSoFar.Length; i++)
            {
                // counts up once per non-repeating letter
                if (correctLetters.Contains(guessedSoFar[i]))
                    counter++;
            }

            // if our counter is equal to the length of our processed string...
            if (counter == correctLetters.Length)
                // ...user guessed all correct letters
                return true;
            else
                // ...user hasn't guessed all correct letters
                return false;
        }

        /// <summary>
        /// Masks the letters that are not guessed already in the word with a "_".
        /// </summary>
        /// <param name="targetWord">The word to guess</param>
        /// <param name="guessedSoFar">The letters that have been guessed so far</param>
        /// <returns>Masked word</returns>
        static string MaskedWord(string targetWord, string guessedSoFar)
        {
            string builtString = "";

            //If letter i is in targetWord, write letter it. Otherwise, write "_".
            for (int i = 0; i < targetWord.Length; i++)
            {
                if (guessedSoFar.Contains(targetWord[i]))
                    builtString += targetWord[i] + " ";
                else
                    builtString += "_ ";
            }

            return builtString;
        }

        /// <summary>
        /// Checks if input is only letters
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>True if it's valid</returns>
        static bool isValid(string input)
        {
            bool valid = true;

            foreach (char i in input)
            {
                // if a non-letter is found, valid is false
                if (!char.IsLetter(i))
                    valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Returns a multi-line string of a hangman stick figure. Puts the masked word in the base of the stand.
        /// </summary>
        /// <param name="maskedWord">The word to be displayed in the base. Masking sugessted if this
        /// is the be where the Word So Far is to be placed, but this function will adapt to any string.</param>
        /// <param name="numGuessesLeft">Number between 0 and 9. 0 -> 8 represents how many turns the play has left
        /// before they lose the game, and 9 will return a victory screen.</param>
        /// <returns>Procedually generated multi-line string of a hangman board with a string in the base.</returns>
        static string printStickman(string maskedWord, int numGuessesLeft)
        {
            //String literals take all spaces into account. Formatting will be off for this array.
            //Array will correspond to numGuessesLeft.

            //Generate upper part of the stand, and the stick figure itself.
            string[] stickFigure = {@"
     ----
    |    |
    |    |
    |    O
    |   /|\
    |   / \",@"
     ----
    |    |
    |    |
    |    O
    |   /|\
    |   /",@"
     ----
    |    |
    |    |
    |    O
    |   /|\
    |",@"
     ----
    |    |
    |    |
    |    O
    |   /|
    |",@"
     ----
    |    |
    |    |
    |    O
    |    |
    |",@"
     ----
    |    |
    |    |
    |    O
    |
    |",@"
     ----
    |    |
    |    |
    |
    |
    |",@"
     ----
    |    |
    |
    |
    |
    |",@"
     ----
    |
    |
    |
    |
    |",@"
     ----
    |
    |
    |
    |
    |"};

            //Generate word-filled base. Note the newlines. Function will return concatenation of these strings.

            //    |_______________________   Crest of base
            //    |                       |  Top of base       \o/   Top of figure     Figure shown on win condition only
            //    | W O R D T O G U E S S |  Middle of base     |    Middle of figure  (numGuessesLeft == 9)
            //    |_______________________|  Bottom of base    / \   Bottom of figure

            string wordBase = "\n    |\n    |"; // \n for new line. Will be procedurally generated from here.
            
            for (int i = 0; i < maskedWord.Length; i++)        //Crest of base
                wordBase += "_";

            wordBase += "_\n    |";                            //Top of base
            for (int i = 0; i < maskedWord.Length; i++)        
                wordBase += " ";
            if(numGuessesLeft == 9)
                wordBase += " |   \\o/";                       //Top of figure on win condition. Extra \ needed
            else                                               //for proper formatting
                wordBase += " |";                              //Same line, but NOT on victory - no figure generated.

            wordBase += "\n    | " + maskedWord + "|";         //Middle of base
            if (numGuessesLeft == 9)
                wordBase += "    |\n";                         //Middle of figure on victory condition.
            else
                wordBase += "\n";

            wordBase += "    |";                               //Bottom of base
            for (int i = 0; i < maskedWord.Length; i++)
                wordBase += "_";
            wordBase += "_|";
            if (numGuessesLeft == 9)
                wordBase += "   / \\";                         //Bottom of figure on win condition.

                return stickFigure[numGuessesLeft] + wordBase;
        }
    }
}
