#include "stdafx.h"
#include "Game.h"

#include <cstdlib> // using rand()
#include <Windows.h>


int Game::GetRand(int pA, int pB)
{
	return rand() % (pB - pA + 1) + pA;
}

void Game::InitGame()
{
	srand((unsigned int)time(NULL));

	piece = GetRand(0, 6);
	rotation = GetRand(0, 3);
	posX = (BOARD_WIDTH / 2) + pieces->GetXInitialPosition(piece, rotation);
	posY = pieces->GetYInitialPosition(piece, rotation);

	nextPiece = GetRand(0, 6);
	nextRotation = GetRand(0, 3);
	nextPosX = BOARD_WIDTH + 5;
	nextPosY = 5;
}

void Game::CreateNewPiece()
{
	piece = nextPiece;
	rotation = nextRotation;
	posX = (BOARD_WIDTH / 2) + pieces->GetXInitialPosition(piece, rotation);
	posY = pieces->GetYInitialPosition(piece, rotation);

	nextPiece = GetRand(0, 6);
	nextRotation = GetRand(0, 3);
}

void Game::DrawPiece(int x, int y, int piece, int rotation)
{
	color color;               

	int pixelsX = board->GetXPosInPixels(x);
	int pixelsY = board->GetYPosInPixels(y);

	for (int i = 0; i < PIECE_BLOCKS; i++)
	{
		for (int j = 0; j < PIECE_BLOCKS; j++)
		{
			color = BLUE;

			if (pieces->GetBlockType(piece, rotation, j, i) != 0)
				IO->DrawRectangle(pixelsX + i * BLOCK_SIZE,
					pixelsY + j * BLOCK_SIZE,
					(pixelsX + i * BLOCK_SIZE) + BLOCK_SIZE - 1,
					(pixelsY + j * BLOCK_SIZE) + BLOCK_SIZE - 1,
					color);
		}
	}
}

void Game::DrawBoard()
{
	int x1 = BOARD_POSITION - (BLOCK_SIZE * (BOARD_WIDTH / 2)) - 1;
	int x2 = BOARD_POSITION + (BLOCK_SIZE * (BOARD_WIDTH / 2));
	int y = screenHeight - (BLOCK_SIZE * BOARD_HEIGHT);

	IO->DrawRectangle(x1 - BOARD_LINE_WIDTH, y, x1, screenHeight - 1, WHITE);

	IO->DrawRectangle(x2, y, x2 + BOARD_LINE_WIDTH, screenHeight - 1, WHITE);

	x1 += 1;
	for (int i = 0; i < BOARD_WIDTH; i++)
	{
		for (int j = 0; j < BOARD_HEIGHT; j++)
		{
			if (!board->IsFreeBlock(i, j))
				IO->DrawRectangle(x1 + i * BLOCK_SIZE,
					y + j * BLOCK_SIZE,
					(x1 + i * BLOCK_SIZE) + BLOCK_SIZE - 1,
					(y + j * BLOCK_SIZE) + BLOCK_SIZE - 1,
					RED);
		}
	}
}

void Game::DrawScene()
{
	DrawBoard();                                                   
	DrawPiece(posX, posY, piece, rotation);                   
	DrawPiece(nextPosX, nextPosY, nextPiece, nextRotation);    
}
