import { notebookColors, type NotebookColor } from "@/constants/notebookColors";
import type { Concept } from "../Concepts/Concept";
import type { StudySetSummary } from "../StudySets/StudySetSummary";

export interface NotebookDetail {
  id: number;
  title: string;
  subject: string | null;
  color: NotebookColor;
  studySets: StudySetSummary[];
  concepts: Concept[];
  updatedAt: string;
  createdAt: string;
}
