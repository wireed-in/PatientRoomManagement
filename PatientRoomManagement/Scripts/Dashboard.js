/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery.ui.layout/jquery.ui.layout.d.ts" />
function dragAndDrop(draggableClass, dropableClass) {
    var draggableBlock = $(draggableClass);
    draggableBlock.draggable({ revert: "invalid" });
    var droppableBlock = $(dropableClass);
    droppableBlock.droppable({ hoverClass: "ui-state-active" });
    droppableBlock.on("drop", function (event, ui) {
        $(this).addClass("ui-state-highlight");
    });
}
dragAndDrop(".patient-block", ".room-block");
