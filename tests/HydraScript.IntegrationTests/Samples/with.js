let v = {
    x: 2;
    y: 1;
}

let vToX = v with { y: 0; }

>>> vToX

>>> {x: 1;} with {}