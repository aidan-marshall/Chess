export type PieceColour = 'White' | 'Black';

export type GameStatus =
    | 'InProgress'
    | 'Checkmate'
    | 'Stalemate'
    | 'DrawByAgreement'
    | 'Resigned';

export interface GameDto {
    id: number;
    fenPosition: string;
    gameState: GameStatus;
    gameResult: string | null;
    drawReason: string | null;
    toMove: PieceColour;
    drawOfferedBy: PieceColour | null;
    fullMoveNumber: number;
    halfMoveClock: number;
    createdAtUtc: string;
    updatedAtUtc: string;
    moves: MoveDto[];
}

export interface MoveDto {
    id: number;
    from: string;
    to: string;
    notation: string;
    pieceColour: PieceColour;
}
