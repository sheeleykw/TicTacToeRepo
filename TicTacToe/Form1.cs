using System;
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
        //X is represented as 1 and O is represented as -1 in the following code
        private DataGridView ticTacToeTable = new DataGridView();
        private Label gameStatusDisplay = new Label();
        private int[,] currentGameState = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        private int currentTurn = 1;
        private bool gameOver = false;
        private Button resetButton = new Button();
        private Button newGameButton = new Button();

        public Form1()
        {
            int tableWidth = 550, tableHeight = 465;

            gameStatusDisplay.Font = new Font("Arial", 25);
            gameStatusDisplay.Size = new Size(tableWidth, 50);
            gameStatusDisplay.Location = new Point(5, 0);
            gameStatusDisplay.Text = "X's turn.";
            gameStatusDisplay.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(gameStatusDisplay);

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
            ticTacToeTable.Location = new Point(5, 50);
            ticTacToeTable.DefaultCellStyle.Font = new Font("Arial", 100);
            ticTacToeTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ticTacToeTable.Size = new Size(tableWidth, tableHeight);
            Controls.Add(ticTacToeTable);

            Shown += Form1_Shown1;
            Size = new Size(tableWidth + 120, tableHeight + 95);
        }

        private void Form1_Shown1(object sender, EventArgs e)
        {
            ticTacToeTable.ClearSelection();
            ticTacToeTable.SelectionChanged += new EventHandler(CellSelected);
        }

        private void CellSelected(object sender, EventArgs e)
        {
            DataGridViewCell selectedCell = ticTacToeTable.CurrentCell;

            if (currentGameState[selectedCell.RowIndex, selectedCell.ColumnIndex] == 0 && !gameOver)
            {
                if (currentTurn == 1)
                {
                    currentGameState[selectedCell.RowIndex, selectedCell.ColumnIndex] = 1;
                    selectedCell.Value = "X";
                    currentTurn = -1;
                    gameStatusDisplay.Text = "O's turn.";
                }
                else if (currentTurn == -1)
                {
                    currentGameState[selectedCell.RowIndex, selectedCell.ColumnIndex] = -1;
                    selectedCell.Value = "O";
                    currentTurn = 1;
                    gameStatusDisplay.Text = "X's turn.";
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
                        MessageBox.Show("X has won.");
                    }
                    else if (currentTurn == 1)
                    {
                        MessageBox.Show("O has won.");
                    }
                }
                else
                {
                    MessageBox.Show("Game tied.");
                }
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
    }
}
