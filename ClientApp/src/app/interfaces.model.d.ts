interface Role {
  id: string,
  name: string
}

interface Category {
  id: string,
  name: string
}

interface Register {
  email: string,
  password: string
}

interface Login {
  username: string,
  password: string
}

interface Note {
  id: string;
  userId: string,
  category: Category,
  title: string,
  text: string
}

interface User {
  id: string;
  firstName: string,
  lastName: string,
  email: string,
  roles: Role[],
  pictureUrl: string,
  birthdate: Date
}
