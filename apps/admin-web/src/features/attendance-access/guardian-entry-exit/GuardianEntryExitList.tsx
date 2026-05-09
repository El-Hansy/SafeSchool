export function DirectionBadge({ direction }: { direction: "Entry" | "Exit" }) {
  return <span data-direction={direction}>{direction}</span>;
}

export function GuardianEntryExitList({ items }: { items: Array<{ id: string; student: string; direction: "Entry" | "Exit"; time: string }> }) {
  return (
    <ul>
      {items.map((item) => (
        <li key={item.id}>
          {item.student} <DirectionBadge direction={item.direction} /> {item.time}
        </li>
      ))}
    </ul>
  );
}

