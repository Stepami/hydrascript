type node = {
    data: number;
    next: node;
}
type list = {
    head: node;
    append: (number,) => void;
    getOdd: () => number[];
}

function makeNode(item: number): node {
    return {
        data: item;
        next: null;
    }
}

let linkedList: list = {
    head: makeNode(1);
    append => (item: number) {
    let tail: node = head
    while ((tail.next) != null) {
        tail = tail.next
    }
    tail.next = makeNode(item)
};
getOdd => (): number[] {
    let result: number[]
    let n = head
    while (n != null) {
        if ((n.data) % 2 != 0) {
            let i = n.data
            result = result ++ [i,]
        }
        n = n.next
    }
    return result
};
}

linkedList.append(3)
linkedList.append(5)
linkedList.append(7)

print(linkedList as string)

let odds = linkedList.getOdd()
print(odds as string)