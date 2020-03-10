# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rails db:seed command (or created alongside the database with db:setup).
#
# Examples:
#
#   movies = Movie.create([{ name: 'Star Wars' }, { name: 'Lord of the Rings' }])
#   Character.create(name: 'Luke', movie: movies.first)

Question.create(value:"What is your country's most famous bridge?", answer: "https://www.youtube.com/watch?v=SNx8B_oE8IY")
Question.create(value:"Where are you from?", answer: "https://www.youtube.com/watch?v=Xz_fesNQkGM")

Culture.create(name:"Brazillian")
Culture.create(name:"Mennonite")