using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class CBot {

    public static void Main(string[] args)
    {
        getLegalMoves(1,57);
        getLegalMoves(1,62);
        getLegalMoves(2,58);
        getLegalMoves(2,61);
        getLegalMoves(3,56);
        getLegalMoves(3,63);
        getLegalMoves(4,60);
        getLegalMoves(5,59);
    }

    public static int[] pieceValues = {100, 320, 330, 500, 900 , 0};
    static bool[] board = {
                        true,true,true,true,true,true,true,true,
                        true,true,true,true,true,true,true,true,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        true,true,true,true,true,true,true,true,
                        true,true,true,true,true,true,true,true,
                    };
    static bool[] notCapturableBoard = {
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        false,false,false,false,false,false,false,false,
                        true,true,true,true,true,true,true,true,
                        true,true,true,true,true,true,true,true,
                                     };

    //VALUE TABLES FOR EACH PIECE
    static sbyte[][] pieceTables = new sbyte[][]
    {
        //Pawn
        new sbyte[]
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
            5, 5, 10, 25, 25, 10, 5, 5,
            0, 0, 0, 25, 25, 0, 0, 0,
            5, 0, -5, 10, 10, -5, 0, 5,
            5, 10, 10, -20, -20, 10, 10, 5,
            0, 0, 0, 0, 0, 0, 0, 0
        },
        // knight
        new sbyte[]
        {
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20, 0, 0, 0, 0, -20, -40,
            -30, 0, 10, 15, 15, 10, 0, -30,
            -30, 5, 15, 20, 20, 15, 5, -30,
            -30, 0, 15, 20, 20, 15, 0, -30,
            -30, 5, 10, 15, 15, 10, 5, -30,
            -40, -20, 0, 5, 5, 0, -20, -40,
            -50, -25, -30, -30, -30, -30, -25, -50,
        },
        // bishop
        new sbyte[]
        {
            -20, -10, -10, -10, -10, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 10, 10, 5, 0, -10,
            -10, 5, 5, 10, 10, 5, 5, -10,
            -10, 0, 10, 10, 10, 10, 0, -10,
            -10, 10, 10, 10, 10, 10, 10, -10,
            -10, 5, 0, 0, 0, 0, 5, -10,
            -20, -10, -10, -10, -10, -10, -10, -20,
        },
        //rook
        new sbyte[]
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            5, 10, 10, 10, 10, 10, 10, 5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            0, 0, 0, 5, 5, 0, 0, 0
        },
        //queen
        new sbyte[]
        {
            -20, -10, -10, -5, -5, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 5, 5, 5, 0, -10,
            -5, 0, 5, 5, 5, 5, 0, -5,
            0, 0, 5, 5, 5, 5, 0, -5,
            -10, 5, 5, 5, 5, 5, 0, -10,
            -10, 0, 5, 0, 0, 0, 0, -10,
            -20, -10, -10, -5, -5, -10, -10, -20
        },
        // king
        new sbyte[]
        {
            -50, -40, -30, -20, -20, -30, -40, -50,
            -30, -20, -10, -10, -10, -10, -20, -30,
            -30, -10, -30, -30, -30, -30, -10, -30,
            -30, -10, -20, -20, -20, -20, -10, -30,
            -30, -10, -10, -10, -10, -10, -10, -30,
            -30, -10, -10, -10, -10, -10, -10, -30,
            -30, -30, -5, -5, -5, -5, -30, -30,
            50, 30, 0, 0, 0, 0, 30, 50
        }


    };

    static sbyte[][] movementValues = new sbyte[][] {
        new sbyte[] {-8},
        new sbyte[] {-17,-15,-10,-6,6,10,15,17}, //DOESNT WORK BECAUSE OF KNIGHT STOOPID

        new sbyte[] {-9,-7,7,9},
        new sbyte[] {-8,-1,1,8}, 
        new sbyte[] {-9,-7,7,9},

        new sbyte[] {-9,-8,-7,-1,1,7,8,9},
    };

    static sbyte[][] knightExceptionValues = new sbyte[][] {
        new sbyte[] {

        }
    };
    /*public Move evalMove(){
        Move eval;
        return eval;
    }*/

    public static byte getLegalMoves(byte piece, byte pos) {
        byte bestMove = 0;
        byte bestValue = 0;
        byte i = 0;
        byte currentMoveOption = 0;
        switch (piece) {
            case 0: //PAWN
                
                break;
            case 1: //KNIGHT
            
                i = 8; // DO IT 8 TIMES
                while (i > 0) {
                    
                    if (pos+movementValues[piece][i-1] <= 0 || pos+movementValues[piece][i-1] >= 63 || notCapturableBoard[pos+movementValues[piece][i-1]] == true) { //SKIP TO NEXT ITERATION IF OUTOFBOUNDS
                        i--;
                        continue;
                        } //CHECK IF MOVE IS OUT OF BOUNDS
                    if (pieceTables[piece][pos+movementValues[piece][i-1]] > bestValue) bestMove = (byte) (pos+movementValues[piece][i-1]); //CHECK VALUE OF SQUARE
                    Console.WriteLine(pos+movementValues[piece][i-1]);
                    i--;
                }
                return bestMove;

            case 2: //BISHOP
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos); //checks the desired file by iterating over each square until it hits an inhabited square
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                return bestMove;
            case 3: //ROOK
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos); //checks the desired file by iterating over each square until it hits an inhabited square
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                return bestMove;
            case 4: //QUEEN
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos); //checks the desired file by iterating over each square until it hits an inhabited square
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                currentMoveOption++;
                bestMove = checkFile(currentMoveOption,piece,bestMove,bestValue,pos);
                return bestMove;
            case 5: //KING
                i = 8; // DO IT 8 TIMES
                while (i > 0) {
                    
                    if (pos+movementValues[piece][i-1] <= 0 || pos+movementValues[piece][i-1] >= 63 || notCapturableBoard[pos+movementValues[piece][i-1]] == true) { //SKIP TO NEXT ITERATION IF OUTOFBOUNDS
                        i--;
                        continue;
                        } //CHECK IF MOVE IS OUT OF BOUNDS
                    if (pieceTables[piece][pos+movementValues[piece][i-1]] > bestValue) bestMove = (byte) (pos+movementValues[piece][i-1]); //CHECK VALUE OF SQUARE
                    Console.WriteLine(pos+movementValues[piece][i-1]);
                    i--;
                }
                return bestMove;
        }
        return 0;
    }
    static byte checkFile(byte currentMoveOption, byte piece, byte bestMove, byte bestValue, byte pos) {
        int i = 7;
        while (i > 0) { //REPEAT ONE SQUARE MOVEMENT UNTIL SQUARE IS ALREADY COLONIZED
            if (pos+(movementValues[piece][currentMoveOption]*(7-i+1)) <= 0 || pos+(movementValues[piece][currentMoveOption]*(7-i+1)) >= 63) break; //CHECK IF MOVE IS OUT OF BOUNDS
            if (notCapturableBoard[pos+(movementValues[piece][currentMoveOption]*(7-i+1))] == true) break; //FIRST CHECK IF OWN PIECE IN THE WAY
            if (pieceTables[1][pos+(movementValues[piece][currentMoveOption]*(7-i+1))] > bestValue) bestMove = (byte) (pos+(movementValues[piece][currentMoveOption]*(7-i+1)));
            if (board[pos+(movementValues[piece][currentMoveOption]*(7-i+1))] == true) break; //THEN CHECK IF ENEMY PIECE CAN BE CAPTURED
            i--;
        }
        Console.WriteLine(bestMove);
        return bestMove;
    }

}

public struct Move {
    byte nextSquare;
    byte prevSquare;
    byte piece;
} 

