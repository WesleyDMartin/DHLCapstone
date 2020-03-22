class CreateJoinTableQuestionsCultures < ActiveRecord::Migration[6.0]
  def change
    create_join_table :questions, :cultures do |t|
      t.index [:question_id, :culture_id]
      t.index [:culture_id, :question_id]
    end
  end
end
