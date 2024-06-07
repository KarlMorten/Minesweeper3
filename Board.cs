using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper3
{
    internal class Board
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Mines { get; private set; }
        private readonly int[] _rowOffset = { -1, -1, -1, 0, 0, 1, 1, 1 };
        private readonly int[] _colOffset = { -1, 0, 1, -1, 1, -1, 0, 1 };

        public Cell[,] Cells { get; private set; }

        public Board(int rows, int columns, int totalmines)
        {
            Height = rows;
            Width = columns;
            Mines = totalmines;
            Cells = new Cell[rows, columns];
            StartingBoard();
        }

        private void StartingBoard()
        {
            Random rnd = new Random();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Cells[i,j]= new Cell();
                }
            }

            int minesPlaced = 0;
            while (minesPlaced < Mines)
            {
                int rowNr = rnd.Next(Height);
                int colNr = rnd.Next(Width);
                if (!Cells[rowNr, colNr].IsMine)
                {
                    Cells[rowNr, colNr].IsMine = true;
                    minesPlaced++;
                }
            }
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (!Cells[i, j].IsMine)
                    {
                        Cells[i, j].MinesNearby = CountMinesNearby(i, j);
                    }
                }
            }
        }

        private int CountMinesNearby(int row, int column)
        {
            int mineCount = 0;
            for (int i = 0; i < _rowOffset.Length; i++)
            {
                int newRow = row + _rowOffset[i];
                int newColumn = column + _colOffset[i];
                if (IsInside(newRow, newColumn) && Cells[newRow,newColumn].IsMine)
                {
                    mineCount++;
                }
            }
            return mineCount;
        }

        private bool IsInside(int row, int column)
        {
            return (row >= 0 && column >= 0 && row< Height && column< Width);
        }

        public void OpenCells(int row, int column)
        {
            if (!IsInside(row, column) || Cells[row, column].IsOpen)
            {
                return;
            }

            Cells[row, column].IsOpen = true;
            if (Cells[row, column].MinesNearby == 0)
            {
                for (int i = 0; i < _rowOffset.Length; i++)
                {
                    int newRow = row + _rowOffset[i];
                    int newColumn = column + _colOffset[i];
                    OpenCells(newRow,newColumn);
                }
            }
        }

        public void OpenAllMines()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (!Cells[i, j].IsOpen && Cells[i, j].IsMine)
                    {
                        Cells[i, j].IsOpen = true;
                    }
                }
            }
        }

        public Grid CreateGrid()
        {
            var boardpanel = new Grid();
            var dimension = new GridLength(35);
            for (int i = 0; i < Height; i++)
            {
                boardpanel.RowDefinitions.Add(new RowDefinition()
                {
                    Height = dimension
                });
            }
            for (int j = 0; j < Width; j++)
            {
                boardpanel.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = dimension
                });
            }
            return boardpanel;
        }

        public bool HasWon()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (!Cells[i, j].IsOpen && !Cells[i, j].IsMine)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
