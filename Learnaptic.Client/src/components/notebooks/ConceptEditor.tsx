import { EditorContent, useEditor, useEditorState } from "@tiptap/react";
import type { JSONContent } from "@tiptap/core";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import StarterKit from "@tiptap/starter-kit";
import TextAlign from "@tiptap/extension-text-align";
import {
  Bold,
  Italic,
  Underline,
  List,
  ListOrdered,
  AlignLeft,
  AlignCenter,
  AlignRight,
  ChevronDown,
} from "lucide-react";

type ConceptEditorProps = {
  content: JSONContent;
};

function ConceptEditor({ content }: ConceptEditorProps) {
  const editor = useEditor({
    extensions: [
      StarterKit,
      TextAlign.configure({
        types: ["heading", "paragraph"],
      }),
    ],
    content,
  });

  if (!editor) {
    return null;
  }

  const editorState = useEditorState({
    editor,
    selector: ({ editor }) => ({
      isBold: editor.isActive("bold"),
      isUnderline: editor.isActive("underline"),
      isItalic: editor.isActive("italic"),

      isHeading2: editor.isActive("heading", { level: 2 }),
      isHeading3: editor.isActive("heading", { level: 3 }),

      isAlignLeft: editor.isActive({ textAlign: "left" }),
      isAlignCenter: editor.isActive({ textAlign: "center" }),
      isAlignRight: editor.isActive({ textAlign: "right" }),

      isBulletList: editor.isActive("bulletList"),
      isOrderedList: editor.isActive("orderedList"),
    }),
  });

  return (
    <div className="rounded-lg border bg-card">
      <div
        role="toolbar"
        aria-label="Text formatting"
        className="flex items-center gap-1 border-b p-1"
      >
        <Button
          type="button"
          variant="ghost"
          aria-label="Bold"
          aria-pressed={editorState?.isBold}
          className={`size-7 ${
            editorState?.isBold
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().toggleBold().run()}
        >
          <Bold aria-hidden="true" />
        </Button>

        <Button
          type="button"
          variant="ghost"
          aria-label="Italic"
          aria-pressed={editorState?.isItalic}
          className={`size-7 ${
            editorState?.isItalic
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().toggleItalic().run()}
        >
          <Italic aria-hidden="true" />
        </Button>

        <Button
          type="button"
          variant="ghost"
          aria-label="Underline"
          aria-pressed={editorState?.isUnderline}
          className={`size-7 ${
            editorState?.isUnderline
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().toggleUnderline().run()}
        >
          <Underline aria-hidden="true" />
        </Button>

        <Separator orientation="vertical" className="mt-1 h-5" />

        <DropdownMenu>
          <DropdownMenuTrigger
            render={
              <Button variant="ghost" className="h-7 gap-1 px-2">
                {editorState?.isHeading2
                  ? "Section Heading"
                  : editorState?.isHeading3
                    ? "Subheading"
                    : "Normal"}

                <ChevronDown aria-hidden="true" />
              </Button>
            }
          />

          <DropdownMenuContent>
            <DropdownMenuItem
              onClick={() => editor.chain().focus().setParagraph().run()}
            >
              Normal
            </DropdownMenuItem>

            <DropdownMenuItem
              onClick={() =>
                editor.chain().focus().setHeading({ level: 2 }).run()
              }
            >
              Section Heading
            </DropdownMenuItem>

            <DropdownMenuItem
              onClick={() =>
                editor.chain().focus().setHeading({ level: 3 }).run()
              }
            >
              Subheading
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>

        <Separator orientation="vertical" className="mt-1 h-5" />

        <Button
          type="button"
          variant="ghost"
          aria-label="Align Left"
          aria-pressed={editorState?.isAlignLeft}
          className={`size-7 ${
            editorState?.isAlignLeft
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().setTextAlign("left").run()}
        >
          <AlignLeft aria-hidden="true" />
        </Button>

        <Button
          type="button"
          variant="ghost"
          aria-label="Align Center"
          aria-pressed={editorState?.isAlignCenter}
          className={`size-7 ${
            editorState?.isAlignCenter
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().setTextAlign("center").run()}
        >
          <AlignCenter aria-hidden="true" />
        </Button>

        <Button
          type="button"
          variant="ghost"
          aria-label="Align Right"
          aria-pressed={editorState?.isAlignRight}
          className={`size-7 ${
            editorState?.isAlignRight
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().setTextAlign("right").run()}
        >
          <AlignRight aria-hidden="true" />
        </Button>

        <Separator orientation="vertical" className="mt-1 h-5" />

        <Button
          type="button"
          variant="ghost"
          aria-label="Bulleted List"
          aria-pressed={editorState?.isBulletList}
          className={`size-7 ${
            editorState?.isBulletList
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().toggleBulletList().run()}
        >
          <List aria-hidden="true" />
        </Button>

        <Button
          type="button"
          variant="ghost"
          aria-label="Numbered List"
          aria-pressed={editorState?.isOrderedList}
          className={`size-7 ${
            editorState?.isOrderedList
              ? "bg-accent text-accent-foreground hover:bg-accent/70"
              : ""
          }`}
          onClick={() => editor.chain().focus().toggleOrderedList().run()}
        >
          <ListOrdered aria-hidden="true" />
        </Button>
      </div>

      <EditorContent editor={editor} className="concept-editor" />
    </div>
  );
}

export default ConceptEditor;
