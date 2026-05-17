import { useEffect, useState } from 'react';
import { Chessboard, PieceDropHandlerArgs } from 'react-chessboard';
import type { GameDto } from './types/api';

export default function App() {
    const [game, setGame] = useState<GameDto | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        fetch('/api/v1/games', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({}),
        })
            .then(res => res.json())
            .then(data => setGame(data))
            .catch(err => setError(err.message));
    }, []);

    function onPieceDrop({ sourceSquare, targetSquare }: PieceDropHandlerArgs) {
        if (!game) return false;

        fetch(`/api/v1/games/${game.id}/moves`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                fromSquare: sourceSquare,
                toSquare: targetSquare,
            }),
        })
            .then(res => res.json())
            .then(data => setGame(data));

        return true;
    }

    if (error) return <div>Error: {error}</div>;
    if (!game) return <div>Loading...</div>;

    return (
        <div style={{ width: '500px', margin: '40px auto' }}>
            <p>Game ID: {game.id}</p>
            <p>To move: {game.toMove}</p>
            <Chessboard
                options={{
                    position: game.fenPosition,
                    onPieceDrop,
                }}
            />
        </div>
    );
}