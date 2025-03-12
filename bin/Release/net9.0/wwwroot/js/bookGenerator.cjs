const { faker, fakerPT_BR, fakerPL, fakerEN } = require('@faker-js/faker');

module.exports = function (callback, locale, userSeed, offset, count, avgLikes, avgReviews) {
    let newFaker;
    // Выбираем инстанс Faker в зависимости от локали
    switch (locale) {
        case 'en':
            newFaker = fakerEN;
            break;
        case 'pt_BR':
            newFaker = fakerPT_BR;
            break;
        case 'pl':
            newFaker = fakerPL;
            break;
        default:
            newFaker = fakerEN; // Если локаль не указана – английский по умолчанию
            break;
    }

    // Если пользовательский seed задан (и не равен 0), используем его, иначе генерируем случайное число
    const seedInput = parseInt(userSeed, 10);
    const seed = (seedInput && seedInput !== 0)
        ? seedInput
        : newFaker.number.int({ min: 1, max: 1000000 });
    newFaker.seed(seed);

    const books = [];
    for (let i = 0; i < count; i++) {
        let index = offset + i + 1;

        const baseLikes = Math.floor(avgLikes);
        const extraLike = newFaker.number.float({ min: 0, max: 1, precision: 0.01 }) < (avgLikes - baseLikes) ? 1 : 0;
        const likes = baseLikes + extraLike;

        const baseReviews = Math.floor(avgReviews);
        const extraReview = newFaker.number.float({ min: 0, max: 1, precision: 0.01 }) < (avgReviews - baseReviews) ? 1 : 0;
        const reviews = baseReviews + extraReview;

        // Создаем массив текстовых отзывов
        const reviewItems = [];
        for (let r = 0; r < reviews; r++) {
            reviewItems.push({
                Author: newFaker.person.fullName(),         // Или newFaker.name.fullName() в более старых версиях
                Text: newFaker.lorem.sentence()            // Короткий отзыв
            });
        }
        

        const book = {
            Index: index,
            ISBN: newFaker.commerce.isbn(),
            Title: newFaker.book.title(),
            Authors: newFaker.book.author(),
            Publisher: newFaker.book.publisher(),
            Genre: newFaker.book.genre(),
            Year: newFaker.number.int({ min: 1970, max: 2024 }),
            Description: faker.lorem.paragraph(),
            Likes: likes,
            Reviews: reviews,
            ReviewItems: reviewItems,
            CoverUrl: `https://picsum.photos/seed/${seed}_${index}/200/300`
           
        };
        books.push(book);
    }

    callback(null, books);
};