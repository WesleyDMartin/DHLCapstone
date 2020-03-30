# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# This file is the source Rails uses to define your schema when running `rails
# db:schema:load`. When creating a new database, `rails db:schema:load` tends to
# be faster and is potentially less error prone than running all of your
# migrations from scratch. Old migrations may fail to apply correctly if those
# migrations use external dependencies or application code.
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema.define(version: 2020_03_30_170508) do

  # These are extensions that must be enabled in order to support this database
  enable_extension "plpgsql"

  create_table "cultures", force: :cascade do |t|
    t.string "name"
    t.datetime "created_at", precision: 6, null: false
    t.datetime "updated_at", precision: 6, null: false
  end

  create_table "cultures_questions", id: false, force: :cascade do |t|
    t.bigint "question_id", null: false
    t.bigint "culture_id", null: false
    t.index ["culture_id", "question_id"], name: "index_cultures_questions_on_culture_id_and_question_id"
    t.index ["question_id", "culture_id"], name: "index_cultures_questions_on_question_id_and_culture_id"
  end

  create_table "questions", force: :cascade do |t|
    t.string "value"
    t.string "answer"
    t.string "text_answer"
    t.datetime "created_at", precision: 6, null: false
    t.datetime "updated_at", precision: 6, null: false
    t.integer "culture_id"
    t.string "videotype"
    t.index ["culture_id"], name: "index_questions_on_culture_id"
  end

end
