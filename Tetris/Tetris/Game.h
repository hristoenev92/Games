#pragma once
#include "Board.h"
#include "Pieces.h"
#include "IO.h"
#include <time.h>

#define WAIT_TIME 700

class Game
{
public:
	Game(Board board, Pieces pieces, IO *IO, int screenHeight);

	void DrawScene();
	void CreateNewPiece();

	int posX, posY;               
	int piece, rotation;          

	int screenHeight;              
	int nextPosX, nextPosY;       
	int nextPiece, nextRotation;  

	Board *board;
	Pieces *pieces;
	IO *IO;

	int GetRand(int pA, int pB);
	void InitGame();
	void DrawPiece(int pX, int pY, int pPiece, int pRotation);
	void DrawBoard();
};

