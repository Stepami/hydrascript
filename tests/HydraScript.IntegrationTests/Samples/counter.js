type Counter = {
    state:number;
}

function next(counter: Counter, step = 1){
    counter.state = counter.state + step
    return counter.state
}

function newCounter(start: number):Counter{
    return { state: start; }
}

let counter = newCounter(4)
>>>counter.next()
>>>counter.next(2)