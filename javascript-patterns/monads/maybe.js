/**
 * Source: https://www.toptal.com/javascript/option-maybe-either-future-monads-js
 * 
 */
import Monad from './monad';

export class Maybe extends Monad {

    pure = (value) => (value === null || value === undefined) ? none : Some(value)

    // flatMap :: # Option a -> (a -> Option b) -> Option b
    flatMap = f => this.constructor.name === 'None' ? none : f(this.value)

    // equals :: # M a -> M a -> boolean
    equals = (x) => this.toString() === x.toString()
}

class None extends Maybe {
    toString() { return 'None' }
}

// Cached None class value
export const none = new None()
Maybe.pure = none.pure

export class Some extends Maybe {
    constructor(value) {
        super();
        this.value = value;
    }

    toString() {
        return `Some(${this.value})`
    }
}