using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        //X is represented as 1 and O is represented as -1
        private DataGridView ticTacToeTable = new DataGridView();
        private Label gameStatusDisplay = new Label();
        private TextBox gameLog = new TextBox(), gameStats = new TextBox();
        private Button resetButton = new Button();

        private Queue lastTenGames = new Queue();
        private int[,] currentGameState = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        private int currentTurn = 1, logCount = 1;
        private bool gameOver = false, gameStarted = false;
        
        public Form1()
        {
            int tableWidth = 550, tableHeight = 465;

            gameStatusDisplay.Font = new Font("Arial", 25, FontStyle.Bold);
            gameStatusDisplay.Size = new Size(tableWidth, 50);
            gameStatusDisplay.Location = new Point(5, 5);
            gameStatusDisplay.Text = "X's Turn";
            gameStatusDisplay.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(gameStatusDisplay);

            gameLog.ReadOnly = true;
            gameLog.Font = new Font("Arial", 10);
            gameLog.Size = new Size(170, tableHeight - 70);
            gameLog.Location = new Point(10 + tableWidth, 55);
            gameLog.Text = "[1]: ";
            gameLog.TextAlign = HorizontalAlignment.Left;
            gameLog.Multiline = true;
            Controls.Add(gameLog);

            gameStats.ReadOnly = true;
            gameStats.Font = new Font("Arial", 10);
            gameStats.Size = new Size(170, 70);
            gameStats.Location = new Point(10 + tableWidth, tableHeight - 15);
            UpdateGameStats();
            gameStats.TextAlign = HorizontalAlignment.Left;
            gameStats.Multiline = true;
            Controls.Add(gameStats);

            resetButton.Font = new Font("Arial", 12);
            resetButton.Size = new Size(170, 50);
            resetButton.Location = new Point(10 + tableWidth, 0);
            resetButton.Text = "Reset Game";
            resetButton.TextAlign = ContentAlignment.MiddleCenter;
            resetButton.Click += ResetButtonClicked;
            Controls.Add(resetButton);

            ticTacToeTable.ColumnCount = 3;
            ticTacToeTable.ColumnHeadersVisible = false;
            ticTacToeTable.AllowUserToResizeColumns = false;
            ticTacToeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ticTacToeTable.RowCount = 3;
            ticTacToeTable.RowHeadersVisible = false;
            ticTacToeTable.AllowUserToResizeRows = false;
            ticTacToeTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            
            ticTacToeTable.ReadOnly = true;
            ticTacToeTable.MultiSelect = false;
            ticTacToeTable.Location = new Point(5, 55);
            ticTacToeTable.DefaultCellStyle.Font = new Font("Arial", 100);
            ticTacToeTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ticTacToeTable.Size = new Size(tableWidth, tableHeight);
            Controls.Add(ticTacToeTable);

            Shown += Form1_Shown1;
            Size = new Size(tableWidth + 200, tableHeight + 100);
        }

        private void Form1_Shown1(object sender, EventArgs e)
        {
            ticTacToeTable.ClearSelection();
            gameStatusDisplay.Focus();
            ticTacToeTable.SelectionChanged += CellSelected;
        }

        private void CellSelected(object sender, EventArgs e)
        {
            DataGridViewCell selectedCell = ticTacToeTable.CurrentCell;

            if (currentGameState[selectedCell.RowIndex, selectedCell.ColumnIndex] == 0 && !gameOver)
            {
                gameStarted = true;
                if (currentTurn == 1)
                {
                    currentGameState[selectedCell.RowIndex, selectedCell.ColumnIndex] = 1;
                    selectedCell.Value = "X";
                    currentTurn = -1;
                    gameStatusDisplay.Text = "O's Turn";
                }
                else if (currentTurn == -1)
                {
                    currentGameState[selectedCell.RowIndex, selectedCell.ColumnIndex] = -1;
                    selectedCell.Value = "O";
                    currentTurn = 1;
                    gameStatusDisplay.Text = "X's Turn";
                }

            }
            CheckGameStatus();
        }

        private void CheckGameStatus()
        {
            ticTacToeTable.CurrentCell.Selected = false;

            if ((CheckIfWon() || CheckIfTied()) && !gameOver)
            {
                gameOver = true;
                if (CheckIfWon())
                {
                    if (currentTurn == -1)
                    {
                        lastTenGames.Enqueue(1);
                        gameStatusDisplay.Text = "X Wins";
                        gameLog.AppendText("X won the game.");
                    }
                    else if (currentTurn == 1)
                    {
                        lastTenGames.Enqueue(-1);
                        gameStatusDisplay.Text = "O Wins";
                        gameLog.AppendText("O won the game.");
                    }
                }
                else
                {
                    lastTenGames.Enqueue(0);
                    gameStatusDisplay.Text = "No One Wins";
                    gameLog.AppendText("Game ended in a tie.");
                }
                IncreaseLog();
                UpdateGameStats();
                resetButton.Text = "New Game";
            }
        }

        private bool CheckIfWon()
        {
            for (int i = 0; i < 3; i++)
            {
                int fullRow = 0;
                for (int j = 0; j < 2; j++)
                {
                    if (currentGameState[i, j] == 0) break;
                    else if (currentGameState[i, j] == currentGameState[i, j + 1])
                    {
                        fullRow += 1;
                    }
                }
                if (fullRow == 2) return true;
            }

            for (int i = 0; i < 3; i++)
            {
                int fullColumn = 0;
                for (int j = 0; j < 2; j++)
                {
                    if (currentGameState[j, i] == 0) break;
                    else if (currentGameState[j, i] == currentGameState[j + 1, i])
                    {
                        fullColumn += 1;
                    }
                }
                if (fullColumn == 2) return true;
            }

            if (currentGameState[0, 0] != 0 && currentGameState[0, 0] == currentGameState[1, 1] && currentGameState[1, 1] == currentGameState[2, 2]) return true;
            if (currentGameState[0, 2] != 0 && currentGameState[0, 2] == currentGameState[1, 1] && currentGameState[1, 1] == currentGameState[2, 0]) return true;

            return false;
        }

        private bool CheckIfTied()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentGameState[i, j] == 0) return false;
                }
            }

            return true;
        }

        private void ResetButtonClicked(object sender, EventArgs e)
        {
            gameStatusDisplay.Focus();

            if (gameStarted)
            {
                if (gameOver)
                {
                    resetButton.Text = "Reset Game";
                    gameOver = false;
                    if (currentTurn == 1) gameStatusDisplay.Text = "X's Turn";
                    else if (currentTurn == -1) gameStatusDisplay.Text = "O's Turn";
                }
                else
                {
                    gameLog.AppendText("Game reset.");
                    IncreaseLog();
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        currentGameState[i, j] = 0;
                        ticTacToeTable.Rows[i].Cells[j].Value = "";
                    }
                }
                gameStarted = false;
            }
        }

        private void IncreaseLog()
        {
            logCount++;
            gameLog.AppendText(Environment.NewLine + "[" + logCount + "]: ");
        }

        private void UpdateGameStats()
        {
            int xWins = 0, oWins = 0, ties = 0;

            if (lastTenGames.Count > 10) lastTenGames.Dequeue();

            foreach (int result in lastTenGames)
            {
                if (result == 1) xWins++;
                else if (result == -1) oWins++;
                else if (result == 0) ties++;
            }
            
            gameStats.Text = "Of the last 10 games." + Environment.NewLine + 
                "X has won " + xWins + " game(s)." + Environment.NewLine + 
                "O has won " + oWins + " game(s)." + Environment.NewLine + 
                ties + " game(s) ended in a tie.";
        }
    }
}
