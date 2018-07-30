#include "stdafx.h"
#include "Board.h"


void Board::InitBoard()
{
	for (int i = 0; i < BOARD_WIDTH; i++)
		for (int j = 0; j < BOARD_HEIGHT; j++)
			board[i][j] = POS_FREE;
}

void Board::StorePiece(int x, int y, int piece, int rotation)
{
	for (int i1 = x, i2 = 0; i1 < x + PIECE_BLOCKS; i1++, i2++)
	{
		for (int j1 = y, j2 = 0; j1 < y + PIECE_BLOCKS; j1++, j2++)
		{
			if (pieces->GetBlockType(piece, rotation, j2, i2) != 0)
				board[i1][j1] = POS_FILLED;
		}
	}
}

bool Board::IsGameOver()
{
	for (int i = 0; i < BOARD_WIDTH; i++)
	{
		if (board[i][0] == POS_FILLED) 
		{ 
			return true; 
		}
	}
	return false;
}

void Board::DeleteLine(int y)
{
	for (int j = y; j > 0; j--)
	{
		for (int i = 0; i < BOARD_WIDTH; i++)
		{
			board[i][j] = board[i][j - 1];
		}
	}
}

void Board::DeletePossibleLines()
{
	for (int j = 0; j < BOARD_HEIGHT; j++)
	{
		int i = 0;
		while (i < BOARD_WIDTH)
		{
			if (board[i][j] != POS_FILLED) break;
			i++;
		}

		if (i == BOARD_WIDTH) DeleteLine(j);
	}
}

bool Board::IsFreeBlock(int x, int y)
{
	if (board[x][y] == POS_FREE) return true; 
		else return false;
}

int Board::GetXPosInPixels(int pos)
{
	return  ((BOARD_POSITION - (BLOCK_SIZE * (BOARD_WIDTH / 2))) + (pos * BLOCK_SIZE));
}

int Board::GetYPosInPixels(int pos)
{
	return ((screenHeight - (BLOCK_SIZE * BOARD_HEIGHT)) + (pos * BLOCK_SIZE));
}

bool Board::IsPossibleMovement(int x, int y, int piece, int rotation)
{
	for (int i1 = x, i2 = 0; i1 < x + PIECE_BLOCKS; i1++, i2++)
	{
		for (int j1 = y, j2 = 0; j1 < y + PIECE_BLOCKS; j1++, j2++)
		{
			if (i1 < 0 ||
				i1 > BOARD_WIDTH - 1 ||
				j1 > BOARD_HEIGHT - 1)
			{
				if (pieces->GetBlockType(piece, rotation, j2, i2) != 0)
					return 0;
			}
			if (j1 >= 0)
			{
				if ((pieces->GetBlockType(piece, rotation, j2, i2) != 0) &&
					(!IsFreeBlock(i1, j1)))
					return false;
			}
		}
	}
	return true;
}


