class AddQuestionCultureIndex < ActiveRecord::Migration[6.0]
  def change
    add_index :cultures_questions, [:question_id, :culture_id], :unique => true
  end
end
