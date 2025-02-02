type node = {
    data: number;
    next: node;
}

let head: node = {
    data: 1;
    next: null;
}

let h1: node = {
    data: 3;
    next: null;
}

head.next = h1

let h2: node = {
    data: 2;
    next: null;
}

h1.next = h2

function searchEven(n: node): number {
    let h = n
    while (h != null) {
        let d = h.data
        >>>d
        if (d % 2 == 0) {
            return d
        }
        h = h.next
    }
    return -1
}

let j = searchEven(head)
>>>"found Even:"
>>>j