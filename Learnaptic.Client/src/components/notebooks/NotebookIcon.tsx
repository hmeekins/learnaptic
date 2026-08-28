import { notebookColors, type NotebookColor } from "@/constants/notebookColors";

interface NotebookIconProps {
  color: NotebookColor;
}

function NotebookIcon({ color }: NotebookIconProps) {
  const colors = notebookColors[color];

  return (
    <svg viewBox="0 0 64 80" className="h-full w-full">
      {/* Solid notebook cover */}
      <rect x="14" y="4" width="46" height="70" rx="5" fill={colors.base} />

      {/* Dark rings */}
      <rect x="10" y="13.5" width="9" height="4" rx="2.5" fill={colors.dark} />
      <rect x="10" y="28.5" width="9" height="4" rx="2.5" fill={colors.dark} />
      <rect x="10" y="43.5" width="9" height="4" rx="2.5" fill={colors.dark} />
      <rect x="10" y="58.5" width="9" height="4" rx="2.5" fill={colors.dark} />

      {/* White label */}
      <rect x="26" y="17" width="23" height="12" rx="2" fill="white" />
    </svg>
  );
}

export default NotebookIcon;
