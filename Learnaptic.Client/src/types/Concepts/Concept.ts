import type { JSONContent } from "@tiptap/core";

export interface Concept {
  id: number;
  title: string;
  content: JSONContent;
}
