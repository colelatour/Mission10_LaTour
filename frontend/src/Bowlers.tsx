import type { bowler } from './types/Bowler';
import { useState, useEffect } from 'react';

function Bowlers() {
  const [bowlers, setBowler] = useState<bowler[]>([]);

  useEffect(() => {
    const fetchBowler = async () => {
      const response = await fetch('https://localhost:5000/api/BowlingLeague');
      const data = await response.json();
      setBowler(data);
    };
    fetchBowler();
  }, []);

  return (
    <>
      <h1> Bowlers List</h1>
      <table>
        <thead>
          <tr>
            <th>Bowler Name </th>
            <th>Team Name </th>
            <th>Address</th>
            <th>City</th>
            <th>State</th>
            <th>Zip</th>
            <th>Phone Number </th>
          </tr>
        </thead>
        <tbody>
          {bowlers.map((b) => (
            <tr key={b.bowlerId}>
              <td>
                {b.bowlerFirstName} {b.bowlerMiddleInit} {b.bowlerLastName}
              </td>
              <td>{b.teamName}</td>
              <td>{b.bowlerAddress}</td>
              <td>{b.bowlerCity}</td>
              <td>{b.bowlerState}</td>
              <td>{b.bowlerZip}</td>
              <td>{b.bowlerPhoneNumber}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}

export default Bowlers;
