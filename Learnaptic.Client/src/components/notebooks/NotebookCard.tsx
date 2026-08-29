import { type NotebookColor } from "@/constants/notebookColors";
import { useState } from "react";
import { EllipsisVertical } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  AlertDialog,
  AlertDialogContent,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogDescription,
  AlertDialogAction,
  AlertDialogFooter,
  AlertDialogCancel,
} from "@/components/ui/alert-dialog";
import NotebookIcon from "./NotebookIcon";
import formatTimeElapsed from "@/utils/formatDate";

interface NotebookCardProps {
  id: number;
  title: string;
  subject?: string;
  color: NotebookColor;
  lastAccessedAt: string;
  onDelete: () => void;
  onClick: () => void;
}

function NotebookCard(props: NotebookCardProps) {
  const dateString = formatTimeElapsed(props.lastAccessedAt);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);

  return (
    <div
      onClick={props.onClick}
      className="flex cursor-pointer items-center gap-4 rounded-xl border bg-card p-4 transition-colors hover:bg-muted/50"
    >
      <div className="h-16 w-14 shrink-0">
        <NotebookIcon color={props.color} />
      </div>

      <div className="min-w-0 flex-1">
        <h2 className="truncate font-semibold">{props.title}</h2>

        {props.subject && (
          <p className="text-sm text-muted-foreground">{props.subject}</p>
        )}

        <p className="mt-1 text-xs text-muted-foreground">
          Last accessed {dateString}
        </p>
      </div>

      <DropdownMenu>
        <DropdownMenuTrigger
          render={
            <Button
              variant="ghost"
              size="icon"
              onClick={(event) => {
                event.stopPropagation();
              }}
            />
          }
        >
          <EllipsisVertical />
        </DropdownMenuTrigger>

        <DropdownMenuContent align="end">
          <DropdownMenuItem
            variant="destructive"
            onClick={(event) => {
              setDeleteDialogOpen(true);
              event.stopPropagation();
            }}
          >
            Delete
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete notebook?</AlertDialogTitle>

            <AlertDialogDescription>
              Are you sure you want to delete your {props.title} notebook? This
              action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>

          <AlertDialogFooter>
            <AlertDialogCancel
              onClick={(event) => {
                event.stopPropagation();
              }}
            >
              Cancel
            </AlertDialogCancel>

            <AlertDialogAction
              onClick={(event) => {
                event.stopPropagation();
                props.onDelete();
              }}
            >
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}

export default NotebookCard;
