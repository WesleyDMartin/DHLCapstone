class AddCultureToQuestion < ActiveRecord::Migration[6.0]
  def change
    add_column :questions, :culture_id, :integer
    add_index  :questions, :culture_id
  end
end
