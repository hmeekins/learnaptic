import type { Concept } from "../../types/Concepts/Concept";

type ConceptTableOfContentsProps = {
  concepts: Concept[];
  selectedConceptId: number | null;
  onSelectConcept: (id: number) => void;
};

function ConceptTableOfContents(props: ConceptTableOfContentsProps) {
  return (
    <aside className="sticky top-28">
      <div className="space-y-3">
        <h2 className="flex justify-center text-sm font-semibold">Contents</h2>

        <div className="max-h-[calc(100vh-8rem)] space-y-1 overflow-y-auto">
          {props.concepts.map((concept) => (
            <button
              key={concept.id}
              type="button"
              onClick={() => props.onSelectConcept(concept.id)}
              className={`w-full truncate rounded-md border px-3 py-2 text-left text-sm ${
                props.selectedConceptId === concept.id
                  ? "border-primary font-medium"
                  : "border-transparent hover:border-border"
              }`}
            >
              {concept.title}
            </button>
          ))}
        </div>
      </div>
    </aside>
  );
}

export default ConceptTableOfContents;
