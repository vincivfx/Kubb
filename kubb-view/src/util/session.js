export default {
    setStored(item) {
        localStorage.setItem('auth', JSON.stringify(item));
    },
    getName() {
        return this.getStored().name;
    },
    getStored() {
        const store = localStorage.getItem('auth');

        if (!store) return false;

        return JSON.parse(store);
    },
    removeStored() {
        localStorage.removeItem('auth');
    }
}