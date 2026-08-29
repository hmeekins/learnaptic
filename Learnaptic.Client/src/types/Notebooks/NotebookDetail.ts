import type { Concept } from "../Concepts/Concept";
import type { StudySetSummary } from "../StudySets/StudySetSummary";

export interface NotebookDetail {
  id: number;
  title: string;
  subject: string | null;
  studySets: StudySetSummary[];
  concepts: Concept[];
  updatedAt: string;
  createdAt: string;
}
