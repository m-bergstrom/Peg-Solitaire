/**
 JavaScript class used to pass drag data back to the C# object.
 */

class DragDropResult {
    constructor(sourceItemId = '', sourceContainerId = '', targetItemId = '', targetContainerId = '') {
        this.sourceItemId = sourceItemId;
        this.sourceContainerId = sourceContainerId;
        this.targetItemId = targetItemId;
        this.targetContainerId = targetContainerId;
    }
}


/**
 JavaScript module that provides simple cross-platform drag and drop functionality to Blazor.
 @constructor
 */

function DragDropInterop() {
    let dragDropHelper;
    let options, draggingElement, rect, sourceContainerId, sourceItemId, raf;
    let x, y, deltaX, deltaY;
    x = y = deltaX = deltaY = 0;

    /**
     Default options which can be overridden by passing in a DropDropOptions object
     */
    const defaults = {
        containerClass: 'dd-container',
        itemClass: 'dd-item',
        handleClass: 'handle',
        sort: true,
        clone: false
    };

    /**
     * Method used to merge DragDropOptions (C#) values with the predefined default option values.
     * Values passed in override the defaults automatically.
     * @param {any} settings
     */
    const assignOptions = function (settings) {
        const target = {};
        Object.keys(defaults).forEach(function (key) {
            target[key] = (Object.prototype.hasOwnProperty.call(settings, key) ? settings[key] : defaults[key]);
        });
        options = target;
    };

    /**
     * Public exported method used to initialize the various options associated with this module and
     * store the DotNet object used to call methods labeled with JSInvokable.
     * @param {any} helper - the specified DotNet object
     * @param {any} jsonOptions - the serialized DragDropOptions object
     */
    this.initialize = function (helper, jsonOptions) {
        dragDropHelper = helper;
        // assign the options
        assignOptions(JSON.parse(jsonOptions));
    };

    /**
     * Method used to add event listeners to all of the draggable items/elements.
     */
    this.addListeners = function () {
        let items = [...document.querySelectorAll(`.${options.itemClass}`)];
        items.forEach(item => {
            item.addEventListener('pointerdown', pointerDown, {passive: false});
            if (!options.handleClass) {
                // change the item's cursor to the move cursor
                item.classList.add('moveCursor');
            } else {
                let handle = item.querySelector(`.${options.handleClass}`);
                if (!!handle) {
                    handle.classList.add('moveCursor');
                }
            }
        });
    };
    /**
     * Method used to remove the event listeners from all draggable items/elements.
     */
    const removeItemListeners = function () {
        let items = [...document.querySelectorAll(`.${options.itemClass}`)];
        items.forEach(item => {
            item.removeEventListener('pointerdown', pointerDown, {passive: false});
        });
    };
    /**
     * Private method called when the left button of a mouse is held down, or a touch screen is touched on
     * a draggable item/element.
     * @param {any} event - a 'pointerdown' event.
     */
    const pointerDown = function (event) {
        event.preventDefault();
        if (options.handleClass && event.target.classList.contains(options.handleClass)) {
            // the handle is presumably within the draggable item, so locate the item itself
            draggingElement = event.target.closest(`.${options.itemClass}`);
        } else {
            if (event.target.classList.contains(options.itemClass)) {
                draggingElement = event.target;
            } else {
                draggingElement = event.target.closest(`.${options.itemClass}`);
            }
        }
        if (!!draggingElement) {
            draggingElement.setPointerCapture(event.pointerId);
            draggingElement.classList.add('dragging');
            x = event.clientX;
            y = event.clientY;
            rect = draggingElement.getBoundingClientRect();
            const container = draggingElement.closest(`.${options.containerClass}`);
            if (container) {
                sourceContainerId = container.dataset.containerId || container.id;
            }
            sourceItemId = draggingElement.dataset.itemId || draggingElement.id;
            document.addEventListener('pointermove', pointerMove, {passive: false});
            document.addEventListener('pointerup', pointerUp, {passive: false});
        }
    }

    /**
     * Private method called when the mouse or pointer device is moved.
     * This method is called very frequently.  The request animation frame API is
     * leveraged to smooth the appearance of dragging the item/element.
     * @param {any} event - a 'pointermove' event.
     */
    const pointerMove = function (event) {
        //  if (!raf) {
        deltaX = event.clientX - x;
        deltaY = event.clientY - y;
        draggingElement.style.transform = `translate3d(${deltaX}px, ${deltaY}px, 0px)`;
//            raf = requestAnimationFrame(pointerMoveRAF);
        //  }
    };
    /**
     * Private method called by the request animation frame that animates the item/element's movement.
     * Once the element is finished rendering the frame variable is released to allow the next rendering.
     */
    const pointerMoveRAF = function () {
        draggingElement.style.transform = `translate3d(${deltaX}px, ${deltaY}px, 0px)`;
        raf = null;
    };
    /**
     * Private method called when the mouse button or pointer device is released (dragging has completed).
     * @param {any} event - 'pointerup' event
     */
    const pointerUp = function (event) {
        // clean up
        document.removeEventListener('pointermove', pointerMove);
        document.removeEventListener('pointerup', pointerUp);
        // if the animation frame rendering was in process when the button or pointer device was release,
        // this will cancel the scheduled rendering.
        if (raf) {
            cancelAnimationFrame(raf);
            raf = null;
        }
        draggingElement.style.transform = 'translate3d(0px,0px,0px)';
        if (deltaX == 0 && deltaY == 0) {
            // item wasn't moved, so ignore the event
            return;
        }
        deltaX = deltaY = 0;
        // locate the container on which the item/element was dropped (if any)
        let element = document.elementFromPoint(event.clientX, event.clientY);
        let container = element.closest(`.${options.containerClass}`);
        if (container) {
            let afterElement;
            if (options.sort) {
                afterElement = getDragAfterElement(container, event.clientY);
            }
            let targetItemId = !!afterElement ? (afterElement.dataset.itemId || afterElement.id) : '';
            // create result object
            let result = new DragDropResult(sourceItemId, sourceContainerId, targetItemId, container.dataset.containerId || container.id);
            // remove the listeners to avoid possible memory leaks
            //           removeItemListeners();
            // pass back the DragDropResult to the C# object
            dragDropHelper.invokeMethodAsync('OnPointerUp', result);
        }
        draggingElement.classList.remove('dragging');
    }
    /**
     * Private method used to determine where the dragged item/element should be placed if sorting is enabled.
     * @param {any} container - the container on which the item/element was dropped.
     * @param {any} y - The y coordinate of the mouse or pointer device when the item was dropped.
     * @returns The element after which the dragged item/element should be inserted.  If the return value is null
     * the dragged item should be appended to end of the container's items.
     */
    const getDragAfterElement = function (container, y) {
        const draggableElements = [...container.querySelectorAll(`.${options.itemClass}:not(.dragging)`)];

        return draggableElements.reduce((closest, child) => {
            const box = child.getBoundingClientRect();
            const offset = y - box.top - box.height / 2;
            if (offset < 0 && offset > closest.offset) {
                return {offset: offset, element: child}
            } else {
                return closest
            }
        }, {offset: Number.NEGATIVE_INFINITY}).element
    }

    /**
     * Destructor method use to try and prevent memory leaks when the module is disposed
     * in the C# object.
     */
    this.destroy = function () {
        options = draggingElement = rect = sourceContainerId = sourceItemId = raf = null;
        x = y = deltaX = deltaY = null;
        removeItemListeners();
        document.removeEventListener('pointermove', pointerMove);
        document.removeEventListener('pointerup', pointerUp);
        dragDropHelper = null;
    }
}

export const {initialize, addListeners, destroy} = new DragDropInterop();