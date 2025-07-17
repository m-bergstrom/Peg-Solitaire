export function getElementSize(id) {
    console.log(`Getting size for element with id ${id}`);
    const elem = document.getElementById(id);
    if (!elem) return null;
    console.log(`Found element with id ${id}`);
    const rect = elem.getBoundingClientRect();
    console.log(`Element with id ${id} is ${rect.width} wide and ${rect.height} tall`);
    return {width: rect.width, height: rect.height};
}